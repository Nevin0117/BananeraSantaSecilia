using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SantaSecilia.Application.Services;
using System.Threading.Tasks;

namespace SantaSecilia.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        [ObservableProperty]
        private string _username = "";
        [ObservableProperty]
        private string _password = "";
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage = "";

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) 
                return;

            try
            {
                IsBusy = true;
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(Username))
                {
                    ErrorMessage = "Por favor ingrese un usuario";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Por favor ingrese una contraseña";
                    return;
                }

                var user = await _authService.ValidateCredentialsAsync(Username, Password);

                if (user != null)
                {
                    await _authService.SetCurrentUserAsync(user.Id);
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    ErrorMessage = "Credenciales inválidas";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
