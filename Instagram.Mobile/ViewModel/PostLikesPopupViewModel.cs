using CommunityToolkit.Mvvm.ComponentModel;
using Instagram.Models.PostLike;

namespace Instagram.Mobile.ViewModel
{
    public partial class PostLikesPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private IEnumerable<PostLikeViewModel> _likes;

        public PostLikesPopupViewModel(IEnumerable<PostLikeModel> likes)
        {
            _likes = likes.Select(x => new PostLikeViewModel(x));
        }
    }
}
