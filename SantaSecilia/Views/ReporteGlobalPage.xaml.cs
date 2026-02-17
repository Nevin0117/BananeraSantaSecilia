using SantaSecilia.Application.Services;

namespace SantaSecilia.Views;

public partial class ReporteGlobalPage : ContentPage
{
    public ReporteGlobalPage()
    {
        InitializeComponent();
        MyHeader.SetActivePage("ReporteGlobal");
    }

    private void SemanaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Datos estáticos de ejemplo
        var datos = new List<object>
        {
            new { Actividad = "Fumigar bolsas",  Horas = "34", Tarifa = "0.7790", Total = "B/. 2,648.60" },
            new { Actividad = "Celador",          Horas = "55", Tarifa = "0.8123", Total = "B/. 4,467.65" },
            new { Actividad = "Cargar Bambú",     Horas = "27", Tarifa = "0.7011", Total = "B/. 1,892.97" },
            new { Actividad = "Soldador",         Horas = "33", Tarifa = "1.0126", Total = "B/. 3,341.58" }
        };

        ReporteCollection.ItemsSource = datos;

        // Mostrar tabla y ocultar mensaje vacío
        MensajeVacioContainer.IsVisible = false;
        ReporteCollection.IsVisible = true;
        TotalPagadoLabel.Text = "B/. 15,271.48";
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Verificar que hay datos cargados
            if (ReporteCollection.ItemsSource == null)
            {
                await DisplayAlertAsync("Error", "Debe seleccionar una semana primero", "OK");
                return;
            }

            // Obtener la semana seleccionada
            var semanaSeleccionada = SemanaPicker.SelectedItem?.ToString() ?? "Sin especificar";

            // Convertir los datos a la clase del generador
            var actividades = new List<ReporteGlobalPDFGenerator.ActividadReporte>
        {
            new() { Actividad = "Fumigar bolsas", Horas = "34", Tarifa = "0.7790", Total = "B/. 2,648.60" },
            new() { Actividad = "Celador",         Horas = "55", Tarifa = "0.8123", Total = "B/. 4,467.65" },
            new() { Actividad = "Cargar Bambú",    Horas = "27", Tarifa = "0.7011", Total = "B/. 1,892.97" },
            new() { Actividad = "Soldador",        Horas = "33", Tarifa = "1.0126", Total = "B/. 3,341.58" }
        };

            var totalPagado = TotalPagadoLabel.Text;

            // Cargar el logo como bytes
            byte[] logoBytes;
            using (var stream = await FileSystem.OpenAppPackageFileAsync("logo.png"))
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                logoBytes = ms.ToArray();
            }

            // Generar el PDF
            var pdfBytes = ReporteGlobalPDFGenerator.GenerarPDF(semanaSeleccionada, actividades, totalPagado, logoBytes);

            // Guardar el archivo en Documentos del usuario
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"ReporteGlobal_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            // Abrir el PDF con el visor predeterminado del sistema
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