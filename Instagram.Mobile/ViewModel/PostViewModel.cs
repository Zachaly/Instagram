using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.View;
using Instagram.Models.Post;

namespace Instagram.Mobile.ViewModel
{
    public partial class PostViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ImageUrl))]
        [NotifyPropertyChangedFor(nameof(CurrentImage))]
        [NotifyPropertyChangedFor(nameof(Created))]
        private PostModel _post;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ImageUrl))]
        [NotifyPropertyChangedFor(nameof(CurrentImage))]
        private int _currentImageIndex = 0;

        public string ImageUrl => $"{Configuration.ImageUrl}post/{Post?.ImageIds.ElementAt(CurrentImageIndex)}";

        public string CurrentImage => $"{CurrentImageIndex + 1}/{Post?.ImageIds.Count()}";

        public string Created 
            => DateTimeOffset.FromUnixTimeMilliseconds(Post?.Created ?? 0).DateTime.ToString("dd.MM.yyyy HH:mm");

        public IEnumerable<string> Tags => Post?.Tags.Select(t => $"#{t}");

        public PostViewModel(PostModel post)
        {
            _post = post;
        }

        [RelayCommand]
        private void ChangeImage(int change)
        {
            if(CurrentImageIndex + change < 0)
            {
                return;
            }
            else if(CurrentImageIndex + change >= Post.ImageIds.Count())
            {
                return;
            }

            CurrentImageIndex += change;
        }

        [RelayCommand]
        private async Task GoToCreatorProfileAsync()
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", Post.CreatorId }
            });
        }
    }
}
