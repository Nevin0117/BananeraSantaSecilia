using SantaSecilia.Views;

namespace SantaSecilia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistroLabor), typeof(RegistroLabor));

            Routing.RegisterRoute(nameof(RegistrarLotesPage), typeof(RegistrarLotesPage));
            Routing.RegisterRoute(nameof(EditarLotesPage), typeof(EditarLotesPage));

            Routing.RegisterRoute(nameof(TrabajadorFormPage), typeof(TrabajadorFormPage));
            Routing.RegisterRoute(nameof(EditarTrabajadorPage), typeof(EditarTrabajadorPage));

        }
    }
}
