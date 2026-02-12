using Microsoft.Extensions.DependencyInjection;

namespace SantaSecilia
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        [Obsolete]
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

    }
}