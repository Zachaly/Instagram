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
        (BindingContext as ChatPageViewModel).StartListeningCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ChatPageViewModel).StopListeningCommand.Execute(null);
    }
}