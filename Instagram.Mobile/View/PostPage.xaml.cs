using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class PostPage : ContentPage
{
	public PostPage(PostPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		(BindingContext as PostPageViewModel).LoadPostCommand.Execute(null);
    }
}