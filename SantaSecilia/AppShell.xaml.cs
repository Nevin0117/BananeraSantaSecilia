using SantaSecilia.Application.Services;
using SantaSecilia.Views;

namespace SantaSecilia
{
    public partial class AppShell : Shell
    {
        private readonly AuthService _authService;

        public AppShell(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(RegistroLabor), typeof(RegistroLabor));

            Routing.RegisterRoute(nameof(LotesPage), typeof(LotesPage));
            Routing.RegisterRoute(nameof(RegistrarLotesPage), typeof(RegistrarLotesPage));
            Routing.RegisterRoute(nameof(EditarLotesPage), typeof(EditarLotesPage));

            Routing.RegisterRoute(nameof(TrabajadoresListPage), typeof(TrabajadoresListPage));
            Routing.RegisterRoute(nameof(TrabajadorFormPage), typeof(TrabajadorFormPage));
            Routing.RegisterRoute(nameof(EditarTrabajadorPage), typeof(EditarTrabajadorPage));

            Routing.RegisterRoute(nameof(BoletaSemanalPage), typeof(BoletaSemanalPage));

            Routing.RegisterRoute(nameof(ActividadesPage), typeof(ActividadesPage));
            Routing.RegisterRoute(nameof(RegistrarActividadPage), typeof(RegistrarActividadPage));
            Routing.RegisterRoute(nameof(EditarActividadPage), typeof(EditarActividadPage));

            Routing.RegisterRoute(nameof(ReporteGlobalPage), typeof(ReporteGlobalPage));

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (await _authService.HasActiveSessionAsync())
            {
                await Shell.Current.GoToAsync("//Home");
            }
        }
    }
}
