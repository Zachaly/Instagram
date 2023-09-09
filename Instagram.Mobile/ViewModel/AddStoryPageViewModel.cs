using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class AddStoryPageViewModel : ObservableObject
    {
        private readonly IUserStoryService _userStoryService;
        private readonly IAuthorizationService _authorizationService;

        public ObservableCollection<string> SelectedImages { get; set; } = new ObservableCollection<string>();

        public AddStoryPageViewModel(IUserStoryService userStoryService, IAuthorizationService authorizationService)
        {
            _userStoryService = userStoryService;
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private void RemoveImage(string image)
        {
            SelectedImages.Remove(image);
        }

        [RelayCommand]
        private async Task SelectImageAsync()
        {
            var image = await MediaPicker.Default.PickPhotoAsync();

            if(image is null)
            {
                return;
            }

            SelectedImages.Add(image.FullPath);
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            await _userStoryService.AddAsync(SelectedImages, _authorizationService.UserData.UserId);
            await Shell.Current.GoToAsync("..");
        }
    }
}
