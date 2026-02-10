namespace SantaSecilia.Views;

public partial class ReporteGlobalPage : ContentPage
{
	public ReporteGlobalPage()
	{
		InitializeComponent();
	}

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Imprimir Reporte", "El reporte se está generando...", "Aceptar");
    }
}