using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.User.Request;

namespace Instagram.Mobile.ViewModel
{
    public partial class UpdateProfilePageViewModel : ObservableObject
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private UpdateUserRequest _updateRequest = new UpdateUserRequest();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading = true;

        public bool IsNotLoading => !IsLoading;

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
            await _userService.UpdateAsync(UpdateRequest);

            await Toast.Make("Profile updated").Show();
        }
    }
}
