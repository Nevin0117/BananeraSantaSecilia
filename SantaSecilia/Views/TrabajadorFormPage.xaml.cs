namespace SantaSecilia.Views;

public partial class TrabajadorFormPage : ContentPage
{
	public TrabajadorFormPage()
	{
        InitializeComponent();
	}
    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}