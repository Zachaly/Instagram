using Instagram.Mobile.ViewModel;
using Mopups.Pages;

namespace Instagram.Mobile.View;

public partial class RelationPopup : PopupPage
{
	public RelationPopup(RelationPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}