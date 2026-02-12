using SantaSecilia.Views;

namespace SantaSecilia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Pantallas de Registro y Edición de Actividades
            Routing.RegisterRoute(nameof(RegistrarActividadPage), typeof(RegistrarActividadPage));
            Routing.RegisterRoute(nameof(EditarActividadPage), typeof(EditarActividadPage));
        }
    }
}
