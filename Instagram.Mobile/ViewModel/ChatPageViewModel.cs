using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.User;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class MessageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DirectMessageModel _message;

        public string Created =>
            DateTimeOffset.FromUnixTimeMilliseconds(Message.Created).DateTime.ToString("dd.MM.yyyy HH:mm");

        public bool IsSender { get; }

        [ObservableProperty]
        private bool _isRead = false;

        public MessageViewModel(DirectMessageModel message, bool isSender)
        {
            _message = message;
            IsSender = isSender;
            IsRead = message.Read;
        }
    }

    [QueryProperty(nameof(UserId), nameof(UserId))]
    public partial class ChatPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private long _userId;

        [ObservableProperty]
        private string _newMessageContent = "";

        public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();

        [ObservableProperty]
        private UserModel _receiver;

        private int _pageIndex = 0;
        private int _pageSize = 5;

        private readonly IDirectMessageService _directMessageService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly IWebsocketService _websocketService;

        public ChatPageViewModel(IDirectMessageService directMessageService, IAuthorizationService authorizationService,
            IUserService userService, IWebsocketService websocketService)
        {
            _directMessageService = directMessageService;
            _authorizationService = authorizationService;
            _userService = userService;
            _websocketService = websocketService;
        }

        [RelayCommand]
        private async Task StartListening()
        {
            await _websocketService.StartConnection("direct-message");

            _websocketService.AddListener("MessageReceived", async (DirectMessageModel message) => await ReadMessage(message));

            _websocketService.AddListener("MessageRead", (long id, bool isRead) =>
            {
                var msg = Messages.FirstOrDefault(m => m.Message.Id == id);
                if (msg is null)
                {
                    return;
                }
                msg.Message.Read = isRead;
                msg.IsRead = isRead;
            });
        }

        [RelayCommand]
        private async Task StopListening()
        {
            await _websocketService.StopConnection();
        }

        [RelayCommand]
        private async Task LoadUser()
        {
            Receiver = await _userService.GetByIdAsync(UserId);
        }

        [RelayCommand]
        private async Task LoadMessagesAsync()
        {
            var messages = await _directMessageService.GetAsync(new GetDirectMessageRequest
            {
                PageIndex = _pageIndex,
                PageSize = _pageSize,
                UserIds = new List<long> { UserId, _authorizationService.UserData.UserId }
            });

            foreach(var message in messages)
            {
                await ReadMessage(message, false);
            }

            _pageIndex++;
        }

        private async Task ReadMessage(DirectMessageModel message, bool addOnEnd = true)
        {
            if (!message.Read && message.SenderId != _authorizationService.UserData.UserId)
            {
                await _directMessageService.UpdateAsync(new UpdateDirectMessageRequest
                {
                    Id = message.Id,
                    Read = true
                });
                message.Read = true;
            }

            if(addOnEnd)
            {
                Messages.Add(new MessageViewModel(message, message.SenderId == _authorizationService.UserData.UserId));
            }
            else
            {
                Messages.Insert(0, new MessageViewModel(message, message.SenderId == _authorizationService.UserData.UserId));
            }
        }

        [RelayCommand]
        private async Task AddMessageAsync()
        {
            try
            {
                await _directMessageService.AddAsync(new AddDirectMessageRequest
                {
                    Content = NewMessageContent,
                    ReceiverId = UserId,
                    SenderId = _authorizationService.UserData.UserId
                });

                NewMessageContent = "";
            }
            catch(InvalidRequestException ex)
            {
                await ToastService.MakeToast(ex.Message);
            }
        }
    }
}
