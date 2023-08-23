using CommunityToolkit.Mvvm.ComponentModel;
using Instagram.Models.Post;

namespace Instagram.Mobile.ViewModel
{
    public partial class PostViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ImageUrl))]
        private PostModel _post;

        public string ImageUrl => $"{Configuration.ImageUrl}post/{Post.ImageIds.First()}";

        public PostViewModel(PostModel post)
        {
            _post = post;
        }
    }
}
