using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class EditarTrabajadorPage : ContentPage
{
    public EditarTrabajadorPage(EditarTrabajadorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}