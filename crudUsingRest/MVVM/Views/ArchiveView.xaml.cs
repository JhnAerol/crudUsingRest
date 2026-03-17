using crudUsingRest.MVVM.Models;
using crudUsingRest.MVVM.ViewModels;

namespace crudUsingRest.MVVM.Views;

public partial class ArchiveView : ContentPage
{
	CRUDViewModel _viewModel;

	public ArchiveView(CRUDViewModel viewModel)
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

    private async void Back_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}