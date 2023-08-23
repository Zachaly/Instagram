using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as MainPageViewModel;
            if(!viewModel.IsAuthorized)
            {
                viewModel.GoToLoginPageCommand.Execute(null);
                return;
            }

            viewModel.LoadPostsCommand.Execute(null);
        }
    }
}