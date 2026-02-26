using SantaSecilia.ViewModels;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Views;

public partial class TrabajadoresListPage : ContentPage
{
    public TrabajadoresListPage(AppDbContext context)
    {
        InitializeComponent();
        BindingContext = new TrabajadoresListViewModel(context);
        MyHeader.SetActivePage("Trabajadores");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TrabajadoresListViewModel vm)
            vm.Recargar();
    }

}