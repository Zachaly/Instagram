using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;

namespace Instagram.Mobile.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private IAuthorizationService _authorizationService;

        public bool IsAuthorized => _authorizationService.IsAuthorized;

        public MainPageViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task GoToLoginPageAsync()
            => await Shell.Current.GoToAsync(nameof(LoginPage));

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authorizationService.LogoutAsync();
            await GoToLoginPageAsync();
        }

        [RelayCommand]
        private async Task GoToProfilePageAsync()
            => await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", _authorizationService.UserData.UserId }
            });
        
    }
}
