using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class LotesPage : ContentPage
{
    public LotesPage()
    {
        InitializeComponent();
        BindingContext = new LotesViewModel();
        MyHeader.SetActivePage("Lotes");
    }
}

