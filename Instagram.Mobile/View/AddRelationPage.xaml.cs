using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class AddRelationPage : ContentPage
{
	public AddRelationPage(AddRelationPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}