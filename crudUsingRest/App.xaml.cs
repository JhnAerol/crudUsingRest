using crudUsingRest.MVVM.ViewModels;
using crudUsingRest.MVVM.Views;

namespace crudUsingRest
{
    public partial class App : Application
    {
        CRUDViewModel viewModel = new CRUDViewModel();
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new MainView(viewModel)));
        }
    }
}