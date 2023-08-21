using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.User.Request;

namespace Instagram.Mobile.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly IAuthorizationService _authorizationService;

        [ObservableProperty]
        private LoginRequest _loginRequest = new LoginRequest
        {
            Password = "",
            Email = ""
        };

        public LoginPageViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task GoToRegisterPageAsync()
            => await Shell.Current.GoToAsync(nameof(RegisterPage));

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                await _authorizationService.AuthorizeAsync(LoginRequest);
            }
            catch(InvalidRequestException ex)
            {
                await Toast.Make(ex.Response.Error).Show();
            }

            if(_authorizationService.IsAuthorized)
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
