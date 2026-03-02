using SantaSecilia.ViewModels;
using SantaSecilia.Application.Services;

namespace SantaSecilia.Views;

public partial class TrabajadoresListPage : ContentPage
{
    public TrabajadoresListPage(WorkerService worker)
    {
        InitializeComponent();
        BindingContext = new TrabajadoresListViewModel(worker);
        MyHeader.SetActivePage("Trabajadores");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TrabajadoresListViewModel vm)
            _ = vm.CargarTrabajadoresAsync();
    }
}