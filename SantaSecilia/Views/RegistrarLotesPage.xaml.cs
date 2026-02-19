using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class RegistrarLotesPage : ContentPage
{
    public RegistrarLotesPage(RegistrarLotesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
