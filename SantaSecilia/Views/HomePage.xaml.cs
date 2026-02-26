using Microsoft.Maui.Controls;
using SantaSecilia.Application.Services;

namespace SantaSecilia.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly AuthService _authService;

        public HomePage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlertAsync("Cerrar Sesión", "¿Está seguro que desea salir?", "Sí", "No");
            if (confirm)
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("//Login");
            }
        }

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
