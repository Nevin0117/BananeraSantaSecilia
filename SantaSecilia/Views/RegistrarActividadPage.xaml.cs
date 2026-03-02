using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class RegistrarActividadPage : ContentPage
{
    private readonly RegistrarActividadViewModel _viewModel;

    public RegistrarActividadPage(RegistrarActividadViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LimpiarCampos();
    }

}