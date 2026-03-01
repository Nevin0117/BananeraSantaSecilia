using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class TrabajadorFormPage : ContentPage
{
    private readonly TrabajadorFormViewModel _viewModel;

    public TrabajadorFormPage(TrabajadorFormViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    // Este método se dispara cada vez que entras a la pantalla
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Limpiamos el formulario para que siempre aparezca vacío al entrar
        _viewModel.LimpiarFormulario();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        // Al navegar hacia atrás, el OnAppearing de la lista se encargará de refrescarla
        await Shell.Current.GoToAsync("..");
    }
}