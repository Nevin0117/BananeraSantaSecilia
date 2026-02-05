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
    }
}
