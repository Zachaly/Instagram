using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.PostComment.Request;
using Instagram.Models.PostLike.Request;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    [QueryProperty(nameof(PostId), nameof(PostId))]
    public partial class PostPageViewModel : PostViewModel
    {
        private readonly IPostCommentService _postCommentService;
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPostLikeService _postLikeService;

        public ObservableCollection<PostCommentViewModel> Comments { get; set; } = new ObservableCollection<PostCommentViewModel>();

        [ObservableProperty]
        private long _postId;

        [ObservableProperty]
        private string _newCommentContent = "";

        [ObservableProperty]
        private IDictionary<string, string[]> _commentValidationErrors = null;

        public PostPageViewModel(IPostCommentService postCommentService, IPostService postService,
            IAuthorizationService authorizationService, IPostLikeService postLikeService) : base(null)
        {
            _postCommentService = postCommentService;
            _postService = postService;
            _authorizationService = authorizationService;
            _postLikeService = postLikeService;
        }

        [RelayCommand]
        private async Task LoadPostAsync()
        {
            Post = await _postService.GetByIdAsync(PostId);
        }

        [RelayCommand]
        private async Task LoadCommentsAsync()
        {
            var comments = await _postCommentService.GetAsync(new GetPostCommentRequest { PostId = Post.Id });

            foreach(var comment in comments.Select(c => new PostCommentViewModel(c)))
            {
                Comments.Add(comment);
            }
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

            if (count > 0)
            {
                await _postLikeService.DeleteAsync(_authorizationService.UserData.UserId, post.Post.Id);
                return;
            }

            await _postLikeService.AddAsync(new AddPostLikeRequest
            {
                PostId = post.Post.Id,
                UserId = _authorizationService.UserData.UserId
            });
        }

        [RelayCommand]
        private async Task ShowPostLikesAsync()
        {
            if (Post.LikeCount < 1)
            {
                return;
            }

            var users = (await _postLikeService.GetAsync(new GetPostLikeRequest { PostId = Post.Id }))
                .Select(like => new UserListPopupViewModel.UserListItem 
                { 
                    Id = like.UserId,
                    UserName = like.UserName,
                });

            await MopupService.Instance.PushAsync(new UserListPopup(new UserListPopupViewModel(users)));
        }

        [RelayCommand]
        private async Task AddCommentAsync()
        {
            var request = new AddPostCommentRequest
            {
                PostId = Post.Id,
                Content = NewCommentContent,
                UserId = _authorizationService.UserData.UserId
            };

            long commentId;

            try
            {
                commentId = await _postCommentService.AddAsync(request);
            }
            catch(InvalidRequestException exception)
            {
                CommentValidationErrors = exception.Response.ValidationErrors;
                return;
            }

            CommentValidationErrors = null;

            var comment = await _postCommentService.GetByIdAsync(commentId);

            Comments.Add(new PostCommentViewModel(comment));

            NewCommentContent = "";
        }
    }
}
