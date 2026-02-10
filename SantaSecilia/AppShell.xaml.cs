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
        }
    }
}
