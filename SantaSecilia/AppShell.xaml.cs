using SantaSecilia.Views;

namespace SantaSecilia
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegistrarLotesPage), typeof(RegistrarLotesPage));
            Routing.RegisterRoute(nameof(EditarLotesPage), typeof(EditarLotesPage));
        }
    }
}
