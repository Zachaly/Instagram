using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void EmailEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        (BindingContext as LoginPageViewModel).LoginRequest.Email = e.NewTextValue;
    }

    private void PasswordEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        (BindingContext as LoginPageViewModel).LoginRequest.Password = e.NewTextValue;
    }
}