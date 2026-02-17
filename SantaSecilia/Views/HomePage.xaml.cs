using Microsoft.Maui.Controls;

namespace SantaSecilia.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//Login");

        async void IraRegistroLab(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//RegistroLaboral");

        async void IraTrabajadores(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//Trabajadores");

        async void IraLotes(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//Lotes");

        async void IraActividades(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//Actividades");

        async void IraBoletaSemanal(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//BoletaSemanal");

        async void IraRepoteG(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//ReporteGlobal");
    }
}
