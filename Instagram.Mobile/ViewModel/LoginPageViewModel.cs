using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.User.Request;

namespace Instagram.Mobile.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private LoginRequest _loginRequest = new LoginRequest
        {
            Password = "",
            Email = ""
        };

        private readonly IAuthorizationService _authorizationService;

        public LoginPageViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task GoToRegisterPageAsync()
            => await NavigationService.GoToPageAsync<RegisterPage>();

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                await _authorizationService.AuthorizeAsync(LoginRequest);
            }
            catch(InvalidRequestException ex)
            {
                await ToastService.MakeToast(ex.Message);
            }

            if(_authorizationService.IsAuthorized)
            {
                await NavigationService.GoBackAsync();
            }
        }
    }
}
