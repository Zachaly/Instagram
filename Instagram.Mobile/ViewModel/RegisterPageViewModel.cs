using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.View;

namespace Instagram.Mobile.ViewModel
{
    public partial class RegisterPageViewModel : ObservableObject
    {
        public RegisterPageViewModel()
        {
        }

        [RelayCommand]
        private async Task GoToLoginPage()
            => await Shell.Current.GoToAsync("..");
    }
}
