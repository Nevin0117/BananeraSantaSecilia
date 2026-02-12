using Microsoft.Maui.Controls;

namespace SantaSecilia.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(){
            InitializeComponent();
        }


        private async void OnBackClicked(object sender, EventArgs e){
            await Shell.Current.GoToAsync("LoginPage");}


        async void IraRegistroLab(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(RegistroLabor));
        }

        async void IraTrabajadores(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(TrabajadoresListPage));
        }

        async void IraLotes(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(LotesPage));
        }

        async void IraActividades(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(ActividadesPage));
        }

        async void IraBoletaSemanal(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(BoletaSemanalPage));
        }

        async void IraRepoteG(object sender, EventArgs e){
            await Shell.Current.GoToAsync(nameof(ReporteGlobalPage));
        }
    }
}
