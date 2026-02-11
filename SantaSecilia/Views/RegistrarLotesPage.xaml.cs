using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace SantaSecilia.Views;

public partial class RegistrarLotesPage : ContentPage
{
    public List<string> Estados { get; set; }

    public string EstadoSeleccionado { get; set; }

    public RegistrarLotesPage()
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
        await Shell.Current.GoToAsync("..");
    }
}