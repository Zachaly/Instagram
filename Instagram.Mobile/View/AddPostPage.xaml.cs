using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class AddPostPage : ContentPage
{
	public AddPostPage(AddPostPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}