using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class LotesPage : ContentPage
{
    private readonly LotesViewModel _viewModel;

    public LotesPage(LotesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        MyHeader.SetActivePage("Lotes");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is LotesViewModel vm)
            await vm.LoadAsync();
    }
}



