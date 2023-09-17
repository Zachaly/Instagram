using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;

namespace Instagram.Mobile.ViewModel
{
    public partial class ShellViewModel : ObservableObject
    {
        private readonly IAuthorizationService _authorizationService;

        public ShellViewModel(IAuthorizationService authorizationService) 
        {
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authorizationService.LogoutAsync();
            await NavigationService.GoToPageAsync<LoginPage>();
        }
    }
}
