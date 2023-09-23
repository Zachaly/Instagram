using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatPageViewModel chatPageViewModel)
	{
		InitializeComponent();
		BindingContext = chatPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		(BindingContext as ChatPageViewModel).LoadUserCommand.Execute(null);
		(BindingContext as ChatPageViewModel).LoadMessagesCommand.Execute(null);
    }

    private void OnScroll(object sender, ItemsViewScrolledEventArgs e)
    {
        var viewModel = (ChatPageViewModel)BindingContext;

        if (e.FirstVisibleItemIndex == 0)
        {
            viewModel.LoadMessagesCommand.Execute(null);
        }
    }
}