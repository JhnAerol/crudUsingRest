using crudUsingRest.MVVM.ViewModels;

namespace crudUsingRest.MVVM.Views;

public partial class MainView : ContentPage
{
	public MainView()
	{
		InitializeComponent();
        BindingContext = new CreateViewModel();
        NavigationPage.SetHasNavigationBar(this, false);
	}

    private async void Update_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateView());
    }

    private async void Create_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateView());
    }
}