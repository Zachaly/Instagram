using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}