using SantaSecilia.Application.Services;
using SantaSecilia.ViewModels;
using System.ComponentModel;

namespace SantaSecilia.Views;

public partial class BoletaSemanalPage : ContentPage
{
    private readonly BoletaSemanalViewModel _viewModel;

    public BoletaSemanalPage(BoletaSemanalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        MyHeader.SetActivePage("BoletaSemanal");

        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BoletaSemanalViewModel vm)
        {
            await vm.CargarDatos();
        }
    }

    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        var viewModel = BindingContext as BoletaSemanalViewModel;
        if (viewModel == null) return;

        // Si la propiedad que cambió fue la fecha seleccionada...
        if (e.PropertyName == nameof(viewModel.FechaSeleccionada))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Buscamos los controles por nombre para evitar errores de compilación
                var placeholder = this.FindByName<Label>("PlaceholderFecha");
                var datePicker = this.FindByName<DatePicker>("DatePickerBoleta");
                var semanaLabel = this.FindByName<Label>("SemanaCalculadaLabel");

                // Si los encontramos, aplicamos los cambios
                if (placeholder != null)
                    placeholder.IsVisible = false;

                if (datePicker != null)
                {
                    datePicker.TextColor = Color.FromArgb("#1E293B");
                    datePicker.Opacity = 1;
                }

                if (semanaLabel != null)
                    semanaLabel.IsVisible = true;
            });
        }
    }
}