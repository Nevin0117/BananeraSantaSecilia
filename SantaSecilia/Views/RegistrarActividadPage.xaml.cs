namespace SantaSecilia.Views;

public partial class RegistrarActividadPage : ContentPage
{
	public RegistrarActividadPage()
	{
		InitializeComponent();
	}

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}