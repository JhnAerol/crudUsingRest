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

    private async void GotoArchive_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ArchiveView(_viewModel));
    }

    private void OpenOption_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        
        if(btn != null)
        {
            _selectedBook = btn.BindingContext as Book;
            optionOverlay.IsVisible = true;
        }
        if (_selectedBook == null)
            return;

    }

    private void SoftDelete_Tapped(object sender, EventArgs e)
    {
        var lbl = sender as Label;

        if (lbl != null)
        {
            _selectedBook.isDeleted = true;

        }

        _viewModel.LoadBooks();
        _viewModel.LoadSoftDeletedBooks();
    }

    private void HardDelete_Tapped(object sender, EventArgs e)
    {
        _viewModel.DeleteBookCommand.Execute(_selectedBook);
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        optionOverlay.IsVisible = false;
    }
}