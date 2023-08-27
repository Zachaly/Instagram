using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile.View;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

    private void TextChanged(object sender, TextChangedEventArgs e)
    {
		(BindingContext as SearchPageViewModel).SearchUsersCommand.Execute(null);
    }
}