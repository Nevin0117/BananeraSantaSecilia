using Microsoft.Maui.Controls;

namespace SantaSecilia.Views;

public partial class EditarLotesPage : ContentPage
{
    public List<string> Estados { get; set; }

    public string EstadoSeleccionado { get; set; }
    public EditarLotesPage()
    {
        InitializeComponent();

        Estados = new List<string>
        {
            "Activo",
            "Inactivo"
        };

        BindingContext = this;
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Lógica para editar el lote
        await Shell.Current.GoToAsync("..");
    }
}
