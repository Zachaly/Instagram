using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class RelationManagementPage : ContentPage
{
	public RelationManagementPage(RelationManagementPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

		(BindingContext as RelationManagementPageViewModel).LoadRelationCommand.Execute(null);
    }
}