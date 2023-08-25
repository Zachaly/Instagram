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

        public string Created 
            => DateTimeOffset.FromUnixTimeMilliseconds(Post.Created).DateTime.ToString("dd.MM.yyyy HH:mm");

        public IEnumerable<string> Tags => Post.Tags.Select(t => $"#{t}");

        public PostViewModel(PostModel post)
        {
            _post = post;
        }
    }
}
