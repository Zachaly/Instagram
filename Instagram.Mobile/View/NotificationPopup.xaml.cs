using Instagram.Mobile.ViewModel;
using Mopups.Pages;

namespace Instagram.Mobile.View;

public partial class NotificationPopup : PopupPage
{
	public NotificationPopup(NotificationPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}