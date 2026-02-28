using SantaSecilia.Application.Services;
using SantaSecilia.Infrastructure.Data;
using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class BoletaSemanalPage : ContentPage
{
    public BoletaSemanalPage(AppDbContext context)
    {
        InitializeComponent();
        BindingContext = new BoletaSemanalViewModel(context);
        MyHeader.SetActivePage("BoletaSemanal");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is BoletaSemanalViewModel vm)
        {
            await vm.CargarDatos();
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
            var semana = vm.SemanaSeleccionada ?? "Semana";

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