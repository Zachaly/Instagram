using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Post.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private IAuthorizationService _authorizationService;
        private readonly IPostService _postService;

        public bool IsAuthorized => _authorizationService.IsAuthorized;

        public ObservableCollection<PostViewModel> Posts { get; set; } = new ObservableCollection<PostViewModel>();

        public MainPageViewModel(IAuthorizationService authorizationService, IPostService postService)
        {
            _authorizationService = authorizationService;
            _postService = postService;
        }

        [RelayCommand]
        private async Task GoToLoginPageAsync()
            => await Shell.Current.GoToAsync(nameof(LoginPage));

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authorizationService.LogoutAsync();
            await GoToLoginPageAsync();
        }

        [RelayCommand]
        private async Task GoToProfilePageAsync()
            => await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", _authorizationService.UserData.UserId }
            });

        [RelayCommand]
        private async Task LoadPosts()
        {
            var request = new GetPostRequest
            {
                CreatorIds = _authorizationService.FollowedUserIds,
                SkipCreators = new long[] { _authorizationService.UserData.UserId }
            };

            var posts = await _postService.GetAsync(request);

            foreach(var post in posts.Select(p => new PostViewModel(p)))
            {
                Posts.Add(post);
            }
        }

    }
}
