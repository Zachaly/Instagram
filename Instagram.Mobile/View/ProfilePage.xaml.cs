using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		(BindingContext as ProfilePageViewModel).LoadUserCommand.Execute(null);
    }
}