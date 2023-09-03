using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class UpdateProfilePage : ContentPage
{
	public UpdateProfilePage(UpdateProfilePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		(BindingContext as UpdateProfilePageViewModel).LoadUserDataCommand.Execute(null);
    }
}