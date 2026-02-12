namespace SantaSecilia.Views;

public partial class ReporteGlobalPage : ContentPage
{
	public ReporteGlobalPage()
	{
		InitializeComponent();
	}

    private void SemanaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Datos estáticos de ejemplo
        var datos = new List<object>
        {
            new { Actividad = "Fumigar bolsas",  Horas = "34", Tarifa = "0.7790", Total = "B/. 2,648.60" },
            new { Actividad = "Celador",          Horas = "55", Tarifa = "0.8123", Total = "B/. 4,467.65" },
            new { Actividad = "Cargar Bambú",     Horas = "27", Tarifa = "0.7011", Total = "B/. 1,892.97" },
            new { Actividad = "Soldador",         Horas = "33", Tarifa = "1.0126", Total = "B/. 3,341.58" }
        };

        ReporteCollection.ItemsSource = datos;

        // Mostrar tabla y ocultar mensaje vacío
        MensajeVacioContainer.IsVisible = false;
        ReporteCollection.IsVisible = true;
        TotalPagadoLabel.Text = "B/. 15,271.48";
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Imprimir Reporte", "El reporte se está generando...", "Ok");
    }
}