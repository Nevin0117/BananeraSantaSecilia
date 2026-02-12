namespace SantaSecilia.Views;

public partial class EditarActividadPage : ContentPage
{
	public EditarActividadPage()
	{
		InitializeComponent();
	}

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}