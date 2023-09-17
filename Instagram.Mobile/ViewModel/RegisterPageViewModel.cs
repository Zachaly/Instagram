using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.User.Request;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Instagram.Mobile.ViewModel
{
    public partial class RegisterPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private RegisterRequest _registerRequest = new RegisterRequest
        {
            Name = "",
            Email = "",
            Nickname = "",
            Password = ""
        };

        [ObservableProperty]
        private string _passwordVerification = "";

        [ObservableProperty]
        private IDictionary<string, string[]> _validationErrors;

        private readonly IUserService _userService;

        public RegisterPageViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task GoToLoginPage()
            => await Shell.Current.GoToAsync("..");

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if(PasswordVerification != RegisterRequest.Password)
            {
                await Toast.Make("Passwords empty or dot not match!", ToastDuration.Short, 16).Show();
                return;
            }

            try
            {
                await _userService.RegisterAsync(RegisterRequest);

                await Toast.Make("Account created", ToastDuration.Short, 16).Show();
                await GoToLoginPage();
            }
            catch(InvalidRequestException ex)
            {
                ValidationErrors = ex.Response.ValidationErrors;
            }
        }
    }
}
