using Microsoft.Maui.Controls;
using SantaSecilia.ViewModels;

namespace SantaSecilia.Views
{
    public partial class RegistroLabor : ContentPage
    {
        public RegistroLabor()
        {
            InitializeComponent();
            BindingContext = new RegistroLaborViewModel();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}