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

        public MessageViewModel(DirectMessageModel message, bool isSender)
        {
            _message = message;
            IsSender = isSender;
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
        private bool _blockLoadingMessages = false;

        private readonly IDirectMessageService _directMessageService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public ChatPageViewModel(IDirectMessageService directMessageService, IAuthorizationService authorizationService,
            IUserService userService)
        {
            _directMessageService = directMessageService;
            _authorizationService = authorizationService;
            _userService = userService;
        }

        [RelayCommand]
        private async Task LoadUser()
        {
            Receiver = await _userService.GetByIdAsync(UserId);
        }

        [RelayCommand]
        private async Task LoadMessagesAsync()
        {
            if (_blockLoadingMessages)
            {
                return;
            }

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

            _blockLoadingMessages = messages.Count() < _pageSize;
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
