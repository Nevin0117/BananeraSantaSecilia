namespace SantaSecilia.Views;

public partial class HeaderView : ContentView
{
	public HeaderView()
	{
		InitializeComponent();

	}

    // Resalta el link activo con subrayado y lo restaura en los demás
    public void SetActivePage(string pageName)
    {
        // Reiniciar todos
        LnkRegistroLaboral.FontAttributes = FontAttributes.None;
        LnkTrabajadores.FontAttributes = FontAttributes.None;
        LnkLotes.FontAttributes = FontAttributes.None;
        LnkActividades.FontAttributes = FontAttributes.None;
        LnkBoletaSemanal.FontAttributes = FontAttributes.None;
        LnkReporteGlobal.FontAttributes = FontAttributes.None;

        LnkRegistroLaboral.TextDecorations = TextDecorations.None;
        LnkTrabajadores.TextDecorations = TextDecorations.None;
        LnkLotes.TextDecorations = TextDecorations.None;
        LnkActividades.TextDecorations = TextDecorations.None;
        LnkBoletaSemanal.TextDecorations = TextDecorations.None;
        LnkReporteGlobal.TextDecorations = TextDecorations.None;

        // Resaltar el activo
        var activoLabel = pageName switch
        {
            "RegistroLaboral" => LnkRegistroLaboral,
            "Trabajadores" => LnkTrabajadores,
            "Lotes" => LnkLotes,
            "Actividades" => LnkActividades,
            "BoletaSemanal" => LnkBoletaSemanal,
            "ReporteGlobal" => LnkReporteGlobal,
            _ => null
        };

        if (activoLabel != null)
        {
            activoLabel.FontAttributes = FontAttributes.Bold;
            activoLabel.TextDecorations = TextDecorations.Underline;
        }
    }

    // Navegación
    private async void NavHome_Tapped(object sender, TappedEventArgs e)
    => await Shell.Current.GoToAsync("//Home");

    private async void NavRegistroLaboral_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//RegistroLaboral");

    private async void NavTrabajadores_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//Trabajadores");

    private async void NavLotes_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//Lotes");

    private async void NavActividades_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//Actividades");

    private async void NavBoletaSemanal_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//BoletaSemanal");

    private async void NavReporteGlobal_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("//ReporteGlobal");

    private async void Logout_Clicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//Login");
}
