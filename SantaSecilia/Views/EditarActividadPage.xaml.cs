using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class EditarActividadPage : ContentPage
{
    public EditarActividadPage(EditarActividadViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}