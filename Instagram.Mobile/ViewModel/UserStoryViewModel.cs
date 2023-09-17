using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.UserStory;

namespace Instagram.Mobile.ViewModel
{
    public partial class UserStoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserStoryModel _story;

        public string ImageUrl => $"{Configuration.ImageUrl}story-image/{Story.Images.First().Id}";

        public UserStoryViewModel(UserStoryModel userStoryModel)
        {
            _story = userStoryModel;
        }

        [RelayCommand]
        private async Task GoToProfileAsync()
            => await NavigationService.GoToProfilePageAsync(Story.UserId);
    }
}
