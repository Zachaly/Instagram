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
        private readonly IUserService _userService;
        [ObservableProperty]
        private RegisterRequest _registerRequest = new RegisterRequest();

        [ObservableProperty]
        private string _passwordVerification = "";

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
                var toast = Toast.Make("Passwords empty or dot not match!", ToastDuration.Short, 16);
                await toast.Show();
                return;
            }

            var isCreated = await _userService.RegisterAsync(RegisterRequest);

            if(isCreated)
            {
                var toast = Toast.Make("Account created", ToastDuration.Short, 16);
                await toast.Show();
                await GoToLoginPage();
            }
        }
    }
}
