using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Post.Request;
using Instagram.Models.PostLike.Request;
using Instagram.Models.UserStory.Request;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IPostService _postService;
        private readonly IPostLikeService _postLikeService;
        private readonly IUserStoryService _userStoryService;
        private const int PageSize = 3;

        public bool BlockLoading { get; set; } = false;

        public bool IsAuthorized => _authorizationService.IsAuthorized;

        public ObservableCollection<PostViewModel> Posts { get; set; } = new ObservableCollection<PostViewModel>();
        public ObservableCollection<UserStoryViewModel> UserStories { get; set; } = new ObservableCollection<UserStoryViewModel>();

        private int _pageIndex = 0;

        public MainPageViewModel(IAuthorizationService authorizationService, IPostService postService,
            IPostLikeService postLikeService, IUserStoryService userStoryService)
        {
            _authorizationService = authorizationService;
            _postService = postService;
            _postLikeService = postLikeService;
            _userStoryService = userStoryService;
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

        [RelayCommand]
        private async Task LoadStoriesAsync()
        {
            if (UserStories.Any())
            {
                return;
            }

            var request = new GetUserStoryRequest
            {
                UserIds = _authorizationService.FollowedUserIds,
            };

            var stories = await _userStoryService.GetAsync(request);

            foreach(var story in stories)
            {
                UserStories.Add(new UserStoryViewModel(story));
            }
        }

        [RelayCommand]
        private async Task GoToPostPageAsync(PostViewModel post)
        {
            await Shell.Current.GoToAsync(nameof(PostPage), new Dictionary<string, object>
            {
                { "PostId", post.Post.Id }
            });
        }

        [RelayCommand]
        private async Task ShowStoriesAsync(long startingUserId)
        {
            var startIndex = UserStories.IndexOf(UserStories.First(x => x.Story.UserId == startingUserId));

            await MopupService.Instance.PushAsync(new UserStoryPopup(new UserStoryPopupViewModel(UserStories, startIndex)));
        }

        [RelayCommand]
        private async Task LikePostAsync(PostViewModel post)
        {
            var getRequest = new GetPostLikeRequest
            {
                PostId = post.Post.Id,
                UserId = _authorizationService.UserData.UserId
            };

            var count = await _postLikeService.GetCountAsync(getRequest);

            if(count > 0)
            {
                await _postLikeService.DeleteAsync(_authorizationService.UserData.UserId, post.Post.Id);
                return;
            }

            await _postLikeService.AddAsync(new AddPostLikeRequest 
            { 
                PostId = post.Post.Id,
                UserId =  _authorizationService.UserData.UserId
            });
        }

        [RelayCommand]
        private async Task ShowPostLikesAsync(PostViewModel post)
        {
            if(post.Post.LikeCount < 1)
            {
                return;
            }

            var likes = (await _postLikeService.GetAsync(new GetPostLikeRequest { PostId = post.Post.Id }))
                .Select(like => new UserListPopupViewModel.UserListItem 
                {
                    Id = like.UserId,
                    UserName = like.UserName,
                });

            await MopupService.Instance.PushAsync(new UserListPopup(new UserListPopupViewModel(likes)));
        }

        [RelayCommand]
        private async Task GoToAddStoryPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddStoryPage));
        }
    }
}
