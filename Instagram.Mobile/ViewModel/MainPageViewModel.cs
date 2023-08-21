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
        public async Task GoToLoginPageAsync()
            => await Shell.Current.GoToAsync(nameof(LoginPage));

        [RelayCommand]
        public async Task LogoutAsync()
        {
            await _authorizationService.LogoutAsync();
            await GoToLoginPageAsync();
        }
    }
}
