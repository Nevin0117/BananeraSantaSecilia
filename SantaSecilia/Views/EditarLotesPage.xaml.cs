using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class EditarLotesPage : ContentPage
{
    private readonly EditarLotesViewModel _viewModel;

    public EditarLotesPage(EditarLotesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}
