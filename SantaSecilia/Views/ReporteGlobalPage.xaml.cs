using SantaSecilia.Application.Services;
using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class ReporteGlobalPage : ContentPage
{
    private readonly ReporteGlobalViewModel _viewModel;

    public ReporteGlobalPage(ReporteGlobalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        MyHeader.SetActivePage("ReporteGlobal");
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        // Lo actualizamos después para usar datos reales
        await DisplayAlertAsync("Info", "Generar PDF con datos reales - implementamos después", "OK");
    }
}