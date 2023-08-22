using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}