using SantaSecilia.ViewModels;
namespace SantaSecilia.Views;

public partial class ActividadesPage : ContentPage
{
    public ActividadesPage()
    {
        InitializeComponent();
        BindingContext = new ActividadesViewModel();
    }
}