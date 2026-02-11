using Microsoft.Extensions.DependencyInjection;

namespace SantaSecilia
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

    }
}