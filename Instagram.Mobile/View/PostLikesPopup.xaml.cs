using Instagram.Mobile.ViewModel;
using Mopups.Pages;

namespace Instagram.Mobile.View;

public partial class PostLikesPopup : PopupPage
{
	public PostLikesPopup(PostLikesPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}