namespace crudUsingRest.MVVM.Views;

public partial class CreateView : ContentPage
{
	public CreateView()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainView());
    }
}