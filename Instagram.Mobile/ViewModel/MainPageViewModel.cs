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
        private readonly IAuthorizationService _authorizationService;
        private readonly IPostService _postService;
        private const int PageSize = 3;

        public bool BlockLoading { get; set; } = false;

        public bool IsAuthorized => _authorizationService.IsAuthorized;

        public ObservableCollection<PostViewModel> Posts { get; set; } = new ObservableCollection<PostViewModel>();

        private int _pageIndex = 0;

        public MainPageViewModel(IAuthorizationService authorizationService, IPostService postService)
        {
            _authorizationService = authorizationService;
            _postService = postService;
        }

        [RelayCommand]
        private async Task GoToLoginPageAsync()
            => await Shell.Current.GoToAsync(nameof(LoginPage));

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
                SkipCreators = new long[] { _authorizationService.UserData.UserId },
                PageIndex = _pageIndex,
                PageSize = PageSize
            };

            var posts = await _postService.GetAsync(request);

            if (!posts.Any())
            {
                BlockLoading = true;
                return;
            }

            foreach(var post in posts.Select(p => new PostViewModel(p)))
            {
                Posts.Add(post);
            }

            _pageIndex++;
        }

    }
}
