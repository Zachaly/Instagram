using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class AddStoryPage : ContentPage
{
	public AddStoryPage(AddStoryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}