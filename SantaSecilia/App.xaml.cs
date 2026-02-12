using Microsoft.Extensions.DependencyInjection;

namespace SantaSecilia
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

    }
}