namespace SantaSecilia.Views;

public partial class EditarTrabajadorPage : ContentPage
{
    public EditarTrabajadorPage()
    {
        InitializeComponent();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}