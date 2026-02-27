using Microsoft.Extensions.DependencyInjection;
using SantaSecilia.Application.Services;

namespace SantaSecilia.Views;

public partial class HeaderView : ContentView
{
    public HeaderView()
    {
        InitializeComponent();

        // Ejecutamos la carga del perfil una vez que el componente ya cargó en pantalla
        this.Loaded += HeaderView_Loaded;
    }

    private async void HeaderView_Loaded(object sender, EventArgs e)
    {
        await CargarPerfilAsync();
    }

    private async Task CargarPerfilAsync()
    {
        try
        {
            
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
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar el perfil en el Header: {ex.Message}");
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

    // METODOS DE NAVEGACION

    public void SetActivePage(string pageName)
    {
        ResetTab(BrdRegistroLaboral, LnkRegistroLaboral);
        ResetTab(BrdTrabajadores, LnkTrabajadores);
        ResetTab(BrdLotes, LnkLotes);
        ResetTab(BrdActividades, LnkActividades);
        ResetTab(BrdBoletaSemanal, LnkBoletaSemanal);
        ResetTab(BrdReporteGlobal, LnkReporteGlobal);

        switch (pageName)
        {
            case "RegistroLaboral": HighlightTab(BrdRegistroLaboral, LnkRegistroLaboral); break;
            case "Trabajadores": HighlightTab(BrdTrabajadores, LnkTrabajadores); break;
            case "Lotes": HighlightTab(BrdLotes, LnkLotes); break;
            case "Actividades": HighlightTab(BrdActividades, LnkActividades); break;
            case "BoletaSemanal": HighlightTab(BrdBoletaSemanal, LnkBoletaSemanal); break;
            case "ReporteGlobal": HighlightTab(BrdReporteGlobal, LnkReporteGlobal); break;
        }
    }

    private void ResetTab(Border border, Label label)
    {
        border.BackgroundColor = Colors.Transparent;
        label.TextColor = Color.FromArgb("#64748B");
    }

    private void HighlightTab(Border border, Label label)
    {
        border.BackgroundColor = Color.FromArgb("#D1FAE5");
        label.TextColor = Color.FromArgb("#065F46");
    }

    private async void NavHome_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//Home");
    private async void NavRegistroLaboral_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//RegistroLaboral");
    private async void NavTrabajadores_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//Trabajadores");
    private async void NavLotes_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//Lotes");
    private async void NavActividades_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//Actividades");
    private async void NavBoletaSemanal_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//BoletaSemanal");
    private async void NavReporteGlobal_Tapped(object sender, TappedEventArgs e) => await Shell.Current.GoToAsync("//ReporteGlobal");

    private async void Logout_Clicked(object sender, EventArgs e)
    {
        bool confirm = await Shell.Current.DisplayAlertAsync("Cerrar Sesión", "¿Está seguro que desea salir?", "Sí", "No");
        if (confirm)
        {
            var authService = Handler?.MauiContext?.Services.GetService<AuthService>();
            if (authService != null)
            {
                await authService.LogoutAsync();
            }
            await Shell.Current.GoToAsync("//Login");
        }
    }
}