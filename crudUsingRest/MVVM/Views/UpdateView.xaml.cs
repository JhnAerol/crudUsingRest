using crudUsingRest.MVVM.Models;
using crudUsingRest.MVVM.ViewModels;

namespace crudUsingRest.MVVM.Views;

public partial class UpdateView : ContentPage
{
    CRUDViewModel _viewModel;

    public UpdateView(CRUDViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        NavigationPage.SetHasNavigationBar(this, false);

        if (_viewModel.SelectedBook != null)
        {
            _viewModel.Title = _viewModel.SelectedBook.title;
            _viewModel.Author = _viewModel.SelectedBook.author;
            _viewModel.ImageBook = _viewModel.SelectedBook.imageBook;
        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        _viewModel.SelectedBook = null;
        await Navigation.PopAsync();
        
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {

        _viewModel.SelectedBook = null;
        await Navigation.PopAsync();
    }
}