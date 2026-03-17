using crudUsingRest.MVVM.Models;
using crudUsingRest.MVVM.ViewModels;
using Microsoft.Maui.Layouts;

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
        var selectedBook = button?.BindingContext as Book;

        if (selectedBook != null)
        {
            _viewModel.SelectedBook = selectedBook;

            await Navigation.PushAsync(new UpdateView(_viewModel));
        }
    }

    private async void Create_Clicked(object sender, EventArgs e)
    {
        _viewModel.Title = null;
        _viewModel.Author = null;
        _viewModel.SelectedBook = null;
        if (_viewModel.SelectedBook != null)
            return;

        await Navigation.PushAsync(new CreateView(_viewModel));
    }

    private async void GotoArchive_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ArchiveView(_viewModel));
    }

    private void OpenOption_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var selectedBook = button?.BindingContext as Book;

        if (selectedBook != null)
        {
            _viewModel.SelectedBook = selectedBook;
            optionOverlay.IsVisible = true;
        }

    }

    private void SoftDelete_Tapped(object sender, EventArgs e)
    {
        optionOverlay.IsVisible = false;
    }

    private void HardDelete_Tapped(object sender, EventArgs e)
    {
        optionOverlay.IsEnabled = false;
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        optionOverlay.IsVisible = false;
    }
}