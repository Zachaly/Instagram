using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile
{
    public partial class App : Application
    {
        public App(ShellViewModel shellViewModel)
        {
            InitializeComponent();

            MainPage = new AppShell(shellViewModel);
        }
    }
}