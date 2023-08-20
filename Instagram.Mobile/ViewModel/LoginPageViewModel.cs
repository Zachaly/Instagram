using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.View;

namespace Instagram.Mobile.ViewModel
{
    public partial class LoginPageViewModel : ObservableObject
    {
        public LoginPageViewModel()
        {
        }

        [RelayCommand]
        private async Task GoToRegisterPage()
            => await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}
