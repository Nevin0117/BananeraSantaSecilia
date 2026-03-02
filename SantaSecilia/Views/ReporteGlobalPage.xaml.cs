using SantaSecilia.Application.Services;
using SantaSecilia.ViewModels;
using System.ComponentModel;
using CommunityToolkit.Maui.Storage;

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
            if (!_viewModel.HayDatos)
            {
                await DisplayAlertAsync("Atención", "No hay datos cargados", "OK");
                return;
            }

            var (pdfBytes, fileNameBase) = await _viewModel.PrepararPDFAsync();

            // Creamos un stream con los bytes del PDF
            using var stream = new MemoryStream(pdfBytes);

            // ESTO ABRE LA VENTANA DE WINDOWS "GUARDAR COMO"
            // El usuario elige "Descargas", "Escritorio" o donde quiera.
            var fileSaveResult = await FileSaver.Default.SaveAsync(fileNameBase, stream);

            if (fileSaveResult.IsSuccessful)
            {
                await DisplayAlertAsync("Éxito", $"Archivo guardado en:\n{fileSaveResult.FilePath}", "OK");

                // Abrir el archivo guardado
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(fileSaveResult.FilePath)
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo guardar: {ex.Message}", "OK");
        }
    }
}