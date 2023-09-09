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
            viewModel.LoadStoriesCommand.Execute(null);
        }

        private void PostListScroll(object sender, ItemsViewScrolledEventArgs e)
        {
            var viewModel = (MainPageViewModel)BindingContext;

            if(viewModel.BlockLoading)
            {
                return;
            }

            if(e.LastVisibleItemIndex == viewModel.Posts.Count - 1) 
            {
                viewModel.LoadPostsCommand.Execute(null);
            }
        }
    }
}