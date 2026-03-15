using crudUsingRest.MVVM.ViewModels;

namespace crudUsingRest.MVVM.Views;

public partial class CreateView : ContentPage
{
    CRUDViewModel _viewModel;

    public CreateView(CRUDViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}