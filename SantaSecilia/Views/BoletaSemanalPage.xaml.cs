using SantaSecilia.Application.Services;

namespace SantaSecilia.Views;

public partial class BoletaSemanalPage : ContentPage
{
    public BoletaSemanalPage()
    {
        InitializeComponent();
        MyHeader.SetActivePage("BoletaSemanal");
    }

    private async void Imprimir_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Datos de prueba para el trabajador
            var nombreTrabajador = "Carlos Mendoza";
            var semana = "04/01/26 - 08/01/26";

            // Datos de prueba de actividades
            var registros = new List<BoletaSemanalPDFGenerator.RegistroActividad>
            {
                new() { Fecha = "04/01/26", Actividad = "Fumigar bolsas", Horas = "8", Tarifa = "0.7790", Monto = "B/. 6.23" },
                new() { Fecha = "05/01/26", Actividad = "Celador", Horas = "10", Tarifa = "0.8123", Monto = "B/. 8.12" },
                new() { Fecha = "06/01/26", Actividad = "Fumigar bolsas", Horas = "7", Tarifa = "0.7790", Monto = "B/. 5.45" },
                new() { Fecha = "07/01/26", Actividad = "Cargar Bambú", Horas = "9", Tarifa = "0.7011", Monto = "B/. 6.31" },
                new() { Fecha = "08/01/26", Actividad = "Soldador", Horas = "6", Tarifa = "1.0126", Monto = "B/. 6.08" }
            };

            var totalDevengado = "B/. 32.19";
            var descuentos = "B/. 2.50";
            var totalAPagar = "B/. 29.69";

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