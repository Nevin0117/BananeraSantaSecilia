using SantaSecilia.ViewModels;
using SantaSecilia.Infrastructure.Data;

namespace SantaSecilia.Views;

public partial class TrabajadorFormPage : ContentPage
{
	public TrabajadorFormPage(AppDbContext context)
	{
        InitializeComponent();
        BindingContext = new TrabajadorFormViewModel(context);
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}