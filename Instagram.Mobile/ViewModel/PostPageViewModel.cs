using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.PostComment.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    [QueryProperty(nameof(PostId), nameof(PostId))]
    public partial class PostPageViewModel : PostViewModel
    {
        private readonly IPostCommentService _postCommentService;
        private readonly IPostService _postService;

        public ObservableCollection<PostCommentViewModel> Comments { get; set; } = new ObservableCollection<PostCommentViewModel>();

        [ObservableProperty]
        private long _postId;
        

        public PostPageViewModel(IPostCommentService postCommentService, IPostService postService) : base(null)
        {
            _postCommentService = postCommentService;
            _postService = postService;
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
    }
}
