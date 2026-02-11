namespace SantaSecilia.Views;

public partial class ActividadesPage : ContentPage
{
    public ActividadesPage()
    {
        InitializeComponent();

        ActividadesCollection.ItemsSource = new List<object>
        {
            new { Nombre = "Banderero", Tarifa = "0.7790", Estado = "Activo" },
            new { Nombre = "Limpiar Empacadora", Tarifa = "0.7790", Estado = "Activo" },
            new { Nombre = "Soldador", Tarifa = "1.0126", Estado = "Inactivo" }
        };
    }

    private async void RegistrarActividad_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        button.BackgroundColor = Color.FromArgb("#F0F0F0");
        await Task.Delay(100);
        button.BackgroundColor = Color.FromArgb("#FFFFFF");

        await DisplayAlertAsync("Registrar Actividad", "Abriendo ventana...", "OK");
    }
}