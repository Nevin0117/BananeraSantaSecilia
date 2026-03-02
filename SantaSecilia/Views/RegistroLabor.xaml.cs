using Microsoft.Maui.Controls;
using SantaSecilia.ViewModels;

namespace SantaSecilia.Views
{
    public partial class RegistroLabor : ContentPage
    {
        private readonly RegistroLaborViewModel _viewModel;

        public RegistroLabor(RegistroLaborViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            MyHeader.SetActivePage("RegistroLaboral");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.InitializeAsync();
        }
    }
}