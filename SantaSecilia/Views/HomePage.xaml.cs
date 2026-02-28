using Microsoft.Maui.Controls;
using SantaSecilia.Application.Services;
using SantaSecilia.ViewModels;

namespace SantaSecilia.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly AuthService _authService;

        private readonly HomeViewModel _viewModel;
        public HomePage(AuthService authService, HomeViewModel viewModel)
        {
            InitializeComponent();
            _authService = authService;
            _viewModel = viewModel;
            BindingContext = _viewModel;
            _viewModel = viewModel;
        }

        private async void HomePage_Loaded(object sender, EventArgs e)
        {
            await CargarPerfilAsync();

            await _viewModel.CargarResumenAsync();
        }

        private async Task CargarPerfilAsync()
        {
            try
            {
                // Obtenemos el servicio inyectado en la app
                var authService = Handler?.MauiContext?.Services.GetService<AuthService>();

                if (authService != null)
                {
                    var user = await authService.GetCurrentUserAsync();

                    if (user != null)
                    {
                        LblUserName.Text = user.FullName;
                        LblUserRole.Text = $"@{user.Username}";
                        LblUserInitial.Text = ObtenerIniciales(user.FullName);
                    }
                    else
                    {
                        LblUserName.Text = "Sesión Expirada";
                        LblUserRole.Text = "";
                        LblUserInitial.Text = "?";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el perfil en el Home: {ex.Message}");
            }
        }

        private string ObtenerIniciales(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "?";

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpper();

            return $"{parts[0].Substring(0, 1)}{parts[1].Substring(0, 1)}".ToUpper();
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
