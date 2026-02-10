namespace SantaSecilia.Views;

public partial class ActividadesPage : ContentPage
{
	public ActividadesPage()
	{
		InitializeComponent();
	}

    private async void RegistrarActividad_Tapped(object sender, TappedEventArgs e)
    {
        var border = (Border)sender;

        // Efecto visual de presionado
        border.BackgroundColor = Color.FromArgb("#F0F0F0");
        await Task.Delay(100);
        border.BackgroundColor = Colors.White;

        // Mensaje al presionar boton
        await DisplayAlertAsync("Registrar Actividad", "Abriendo ventana...", "OK");
    }
}