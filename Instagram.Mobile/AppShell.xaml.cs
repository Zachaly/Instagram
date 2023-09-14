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
            Routing.RegisterRoute(nameof(PostPage), typeof(PostPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(AddStoryPage), typeof(AddStoryPage));
            Routing.RegisterRoute(nameof(AddRelationPage), typeof(AddRelationPage));
            BindingContext = viewModel;
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (args.Source == ShellNavigationSource.ShellSectionChanged)
            {
                var navigation = Current.Navigation;
                var pages = navigation.NavigationStack;
                for (var i = pages.Count - 1; i >= 1; i--)
                {
                    navigation.RemovePage(pages[i]);
                }
            }
        }
    }
}