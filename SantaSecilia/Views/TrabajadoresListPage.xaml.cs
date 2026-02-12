using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class TrabajadoresListPage : ContentPage
{
    public TrabajadoresListPage()
    {
        InitializeComponent();
        BindingContext = new TrabajadoresListViewModel();
    }
}