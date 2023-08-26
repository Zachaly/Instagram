using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Post.Request;
using Instagram.Models.User;
using Instagram.Models.UserFollow.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    [QueryProperty(nameof(UserId), "UserId")]
    public partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
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

        public ObservableCollection<PostViewModel> Posts { get; } = new ObservableCollection<PostViewModel>();

        public bool IsNotLoading => !IsLoading;

        private readonly IUserService _userService;
        private readonly IUserFollowService _userFollowService;
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;

        public string ImageUrl => $"{Configuration.ApiUrl}image/profile/{UserId}";

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
    }
}
