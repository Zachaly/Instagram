using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.User.Request;

namespace Instagram.Mobile.ViewModel
{
    public partial class UpdateProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private UpdateUserRequest _updateRequest = new UpdateUserRequest();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading = true;

        [ObservableProperty]
        private IDictionary<string, string[]> _validationErrors = null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNewProfilePictureSelected))]
        private string _newProfilePicture = null;

        public string CurrentProfilePicture => $"{Configuration.ImageUrl}profile/{_authorizationService.UserData.UserId}";

        public bool IsNotLoading => !IsLoading;

        public bool IsNewProfilePictureSelected => NewProfilePicture is not null;

        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public UpdateProfilePageViewModel(IAuthorizationService authorizationService, IUserService userService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
        }

        [RelayCommand]
        private async Task LoadUserDataAsync()
        {
            IsLoading = true;

            var user = await _userService.GetByIdAsync(_authorizationService.UserData.UserId);

            ValidationErrors = null;

            UpdateRequest = new UpdateUserRequest
            {
                Id = user.Id,
                Bio = user.Bio,
                Name = user.Name,
                Nickname = user.Nickname,
            };

            IsLoading = false;
        }

        [RelayCommand]
        private async Task UpdateProfileAsync()
        {
            try
            {
                await _userService.UpdateAsync(UpdateRequest);
                await Toast.Make("Profile updated").Show();
                ValidationErrors = null;
            }
            catch(InvalidRequestException exception)
            {
                ValidationErrors = exception.Response.ValidationErrors;
            }
        }

        [RelayCommand]
        private async Task SelectPictureAsync()
        {
            var file = await MediaPicker.PickPhotoAsync();

            if(file is not null)
            {
                NewProfilePicture = file.FullPath;
            }
        }

        [RelayCommand]
        private async Task UpdateProfilePictureAsync()
        {
            await _userService.UpdateProfilePictureAsync(_authorizationService.UserData.UserId, NewProfilePicture);
            await Toast.Make("Picture updated").Show();
        }
    }
}
