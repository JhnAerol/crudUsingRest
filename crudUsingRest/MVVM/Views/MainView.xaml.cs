using crudUsingRest.MVVM.Models;
using crudUsingRest.MVVM.ViewModels;

namespace crudUsingRest.MVVM.Views;

public partial class MainView : ContentPage
{
    CRUDViewModel _viewModel;
    Book _selectedBook;

	public MainView(CRUDViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
	}

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedBook = button?.CommandParameter as Book;

        if (selectedBook != null)
        {
            _viewModel.SelectedBook = selectedBook;

            await Navigation.PushAsync(new UpdateView(_viewModel));
        }
    }

    private async void Create_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateView(_viewModel));
    }
}