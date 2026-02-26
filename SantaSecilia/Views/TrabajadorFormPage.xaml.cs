using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class TrabajadorFormPage : ContentPage
{
    public TrabajadorFormPage(TrabajadorFormViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}