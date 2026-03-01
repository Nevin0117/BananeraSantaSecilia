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


    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            var vm = BindingContext as BoletaSemanalViewModel;

            if (vm == null)
                return;

            var nombreTrabajador = vm.TrabajadorSeleccionado ?? "Trabajador";
            var semana = vm.RangoSemana ?? "Semana";

            var registros = vm.Filas.Select(f => new BoletaSemanalPDFGenerator.RegistroActividad
            {
                Fecha = f.Fecha.ToString("dd/MM/yy"),
                Actividad = f.Actividad,
                Horas = f.Horas.ToString(),
                Tarifa = f.Tarifa.ToString(),
                Monto = $"B/. {f.Monto:F2}"
            }).ToList();

            var totalDevengado = $"B/. {vm.TotalDevengado:F2}";
            var descuentos = $"B/. {vm.Descuentos:F2}";
            var totalAPagar = $"B/. {vm.TotalPagar:F2}";

            // Cargar el logo
            byte[] logoBytes;
            using (var stream = await FileSystem.OpenAppPackageFileAsync("logo.png"))
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            // Generar el PDF
            var pdfBytes = BoletaSemanalPDFGenerator.GenerarPDF(
                nombreTrabajador,
                semana,
                registros,
                totalDevengado,
                descuentos,
                totalAPagar,
                logoBytes);

            // Guardar el archivo
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"BoletaSemanal_{nombreTrabajador.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            // Abrir el PDF
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });

            await DisplayAlertAsync("Éxito", $"Boleta generada y guardada en:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo generar la boleta:\n{ex.Message}", "OK");
        }
    }
}