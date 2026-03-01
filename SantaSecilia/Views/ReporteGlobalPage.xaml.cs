using SantaSecilia.Application.Services;
using SantaSecilia.ViewModels;
using System.ComponentModel;

namespace SantaSecilia.Views;

public partial class ReporteGlobalPage : ContentPage
{
    private readonly ReporteGlobalViewModel _viewModel;

    public ReporteGlobalPage(ReporteGlobalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        MyHeader.SetActivePage("ReporteGlobal");

        
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }



    private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Si la propiedad que cambió fue la fecha seleccionada...
        if (e.PropertyName == nameof(_viewModel.FechaSeleccionada))
        {
            
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Ocultamos el placeholder falso
                PlaceholderLabel.IsVisible = false;

                // Texto de la fecha del DatePicker nativo
                DateSelector.TextColor = Color.FromArgb("#1E293B");
                DateSelector.Opacity = 1;

                //Texto de semana calculada visible
                SemanaCalculadaLabel.IsVisible = true;
            });
        }
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verificar que hay datos cargados
            if (!_viewModel.HayDatos)
            {
                await DisplayAlertAsync("Error", "Debe seleccionar una fecha para generar el reporte", "OK");
                return;
            }

            var semana = _viewModel.RangoSemana ?? "Semana";

            // Convertir los datos del ViewModel al formato del generador de PDF
            var actividades = _viewModel.Actividades
                .Select(a => new ReporteGlobalPDFGenerator.ActividadReporte
                {
                    Actividad = a.Actividad,
                    Horas = a.Horas,
                    Tarifa = a.Tarifa,
                    Total = a.Total
                }).ToList();

            // Calcular la semana del lunes al viernes
            var fecha = _viewModel.FechaSeleccionada;
            int diff = (7 + (fecha.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime lunes = fecha.AddDays(-diff);
            DateTime viernes = lunes.AddDays(4);
            var semanaTexto = $"{lunes:dd/MM/yy} - {viernes:dd/MM/yy}";

            var totalPagado = _viewModel.TotalPagado;

            // Cargar el logo como bytes
            byte[] logoBytes;
            using (var stream = await FileSystem.OpenAppPackageFileAsync("logo.png"))
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            // Generar el PDF
            var pdfBytes = ReporteGlobalPDFGenerator.GenerarPDF(semanaTexto, actividades, totalPagado, logoBytes);

            // Guardar el archivo
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"ReporteGlobal_{lunes:yyyyMMdd}_{viernes:yyyyMMdd}.pdf";
            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            // Abrir el PDF
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });

            await DisplayAlertAsync("Éxito", $"PDF generado y guardado en:\n{filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo generar el PDF:\n{ex.Message}", "OK");
        }
    }
}