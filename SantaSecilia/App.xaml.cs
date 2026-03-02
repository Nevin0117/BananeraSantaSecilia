using Microsoft.Extensions.DependencyInjection;

namespace SantaSecilia
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = _serviceProvider.GetRequiredService<AppShell>();
            return new Window(shell);
        }
    }
}