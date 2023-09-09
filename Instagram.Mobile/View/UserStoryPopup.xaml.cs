using Instagram.Mobile.ViewModel;
using Mopups.Pages;

namespace Instagram.Mobile.View;

public partial class UserStoryPopup : PopupPage
{
	public UserStoryPopup(UserStoryPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}