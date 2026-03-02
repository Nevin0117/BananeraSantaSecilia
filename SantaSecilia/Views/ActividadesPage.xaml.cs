using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class ActividadesPage : ContentPage
{
    private readonly ActividadesViewModel _viewModel;

    public ActividadesPage(ActividadesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        MyHeader.SetActivePage("Actividades");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Recargar actividades al volver
        await _viewModel.CargarActividadesAsync();
    }
}