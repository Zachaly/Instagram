using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Post.Request;
using Instagram.Models.User;
using Instagram.Models.UserFollow.Request;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    [QueryProperty(nameof(UserId), "UserId")]
    public partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanUnfollow))]
        [NotifyPropertyChangedFor(nameof(IsNotCurrentUser))]
        private long _userId;

        [ObservableProperty]
        private UserModel _userModel;

        [ObservableProperty]
        private int _followersCount = 1;

        [ObservableProperty]
        private int _followingCount = 2;

        [ObservableProperty]
        private int _postCount = 3;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading = true;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanUnfollow))]
        private bool _canFollow;

        public ObservableCollection<PostViewModel> Posts { get; } = new ObservableCollection<PostViewModel>();

        public bool IsNotCurrentUser => _authorizationService.UserData.UserId != UserId;
        public bool CanUnfollow => IsNotCurrentUser && !CanFollow;
        public bool IsNotLoading => !IsLoading;
        public string ImageUrl => $"{Configuration.ApiUrl}image/profile/{UserId}";

        private readonly IUserService _userService;
        private readonly IUserFollowService _userFollowService;
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;

        public ProfilePageViewModel(IUserService userService, IUserFollowService userFollowService, IPostService postService,
            IAuthorizationService authorizationService)
        {
            _userService = userService;
            _userFollowService = userFollowService;
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task LoadUser()
        {
            if(UserId == default)
            {
                UserId = _authorizationService.UserData.UserId;
            }

            IsLoading = true;
            try
            {
                UserModel = await _userService.GetByIdAsync(UserId);
            }
            catch(NotFoundException ex)
            {
                await Toast.Make(ex.Message).Show();
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                return;
            }
            
            PostCount = await _postService.GetCountAsync(new GetPostRequest { CreatorId = UserId });
            FollowersCount = await _userFollowService.GetCountAsync(new GetUserFollowRequest { FollowedUserId = UserId });
            FollowingCount = await _userFollowService.GetCountAsync(new GetUserFollowRequest { FollowingUserId = UserId });
            
            var posts = await _postService.GetAsync(new GetPostRequest { CreatorId = UserId });
            Posts.Clear();
            foreach(var post in posts)
            {
                Posts.Add(new PostViewModel(post));
            }

            CanFollow = !_authorizationService.FollowedUserIds.Contains(UserId)
                && IsNotCurrentUser;

            IsLoading = false;
        }

        [RelayCommand]
        private async Task GoToPostPageAsync(PostViewModel post)
        {
            await Shell.Current.GoToAsync(nameof(PostPage), new Dictionary<string, object>
            {
                { "PostId", post.Post.Id },
            });
        }

        [RelayCommand]
        private async Task FollowAsync()
        {
            var request = new AddUserFollowRequest
            {
                FollowedUserId = UserId,
                FollowingUserId = _authorizationService.UserData.UserId
            };

            await _userFollowService.AddFollowAsync(request);

            _authorizationService.FollowedUserIds.Add(request.FollowedUserId);

            CanFollow = false;
        }

        [RelayCommand]
        private async Task DeleteFollowAsync()
        {
            await _userFollowService.DeleteFollowAsync(_authorizationService.UserData.UserId, UserId);

            _authorizationService.FollowedUserIds.Remove(UserId);

            CanFollow = true;
        }

        [RelayCommand]
        private async Task ShowFollowersAsync()
        {
            var users = (await _userFollowService.GetAsync(new GetUserFollowRequest
            {
                FollowedUserId = UserId,
                JoinFollower = true
            })).Select(follow => new UserListPopupViewModel.UserListItem 
            {
                Id = follow.FollowingUserId,
                UserName = follow.UserName
            });

            await MopupService.Instance.PushAsync(new UserListPopup(new UserListPopupViewModel(users)));
        }


        [RelayCommand]
        private async Task ShowFollowedAsync()
        {
            var users = (await _userFollowService.GetAsync(new GetUserFollowRequest
            {
                FollowingUserId = UserId,
                JoinFollowed = true
            })).Select(follow => new UserListPopupViewModel.UserListItem
            {
                Id = follow.FollowedUserId,
                UserName = follow.UserName
            });

            await MopupService.Instance.PushAsync(new UserListPopup(new UserListPopupViewModel(users)));
        }
    }
}
