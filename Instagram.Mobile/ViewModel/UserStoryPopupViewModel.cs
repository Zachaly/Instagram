using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Mopups.Services;

namespace Instagram.Mobile.ViewModel
{
    public partial class UserStoryPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentStory))]
        [NotifyPropertyChangedFor(nameof(CurrentImageIndex))]
        private int _currentStoryIndex = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentImageUrl))]
        private int _currentImageIndex = 0;

        [ObservableProperty]
        private IEnumerable<UserStoryViewModel> _stories;

        public UserStoryViewModel CurrentStory => Stories.ElementAt(CurrentStoryIndex);

        public string CurrentImageUrl => $"{Configuration.ImageUrl}story-image/{CurrentStory.Story.Images.ElementAt(CurrentImageIndex).Id}";

        public UserStoryPopupViewModel(IEnumerable<UserStoryViewModel> stories, int startingIndex)
        {
            _stories = stories;
            CurrentStoryIndex = startingIndex;
        }

        [RelayCommand]
        private async Task ChangeImageAsync(int count)
        {
            if(CurrentImageIndex + count >= CurrentStory.Story.Images.Count())
            {
                if(CurrentStoryIndex + 1 < Stories.Count())
                {
                    CurrentStoryIndex += 1;
                    CurrentImageIndex = 0;
                } 
                else
                {
                    await PopupService.CloseAsync();
                }
                return;
            }

            if(CurrentImageIndex + count < 0)
            {
                if(CurrentStoryIndex - 1 >= 0)
                {
                    CurrentStoryIndex -= 1;
                    CurrentImageIndex = 0;
                }
                return;
            }

            CurrentImageIndex += count;
        }
    }
}
