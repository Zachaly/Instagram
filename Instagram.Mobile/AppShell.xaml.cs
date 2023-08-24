using Instagram.Mobile.View;
using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell(ShellViewModel viewModel)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            BindingContext = viewModel;
        }
    }
}