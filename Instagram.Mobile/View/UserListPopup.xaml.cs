using Instagram.Mobile.ViewModel;
using Mopups.Pages;

namespace Instagram.Mobile.View;

public partial class UserListPopup : PopupPage
{
	public UserListPopup(UserListPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}