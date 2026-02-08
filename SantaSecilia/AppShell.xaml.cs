using SantaSecilia.Views;

namespace SantaSecilia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TrabajadorFormPage), typeof(TrabajadorFormPage));
            Routing.RegisterRoute(nameof(EditarTrabajadorPage), typeof(EditarTrabajadorPage));
        }
    }
}
