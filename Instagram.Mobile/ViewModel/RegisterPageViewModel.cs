using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.User.Request;

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
            => await NavigationService.GoBackAsync();

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if(PasswordVerification != RegisterRequest.Password)
            {
                await ToastService.MakeToast("Passwords empty or dot not match!");
                return;
            }

            try
            {
                await _userService.RegisterAsync(RegisterRequest);

                await ToastService.MakeToast("Account created");
                await GoToLoginPage();
            }
            catch(InvalidRequestException ex)
            {
                ValidationErrors = ex.Response.ValidationErrors;
            }
        }
    }
}
