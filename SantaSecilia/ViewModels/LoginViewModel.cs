using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace SantaSecilia.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username = "";
        [ObservableProperty]
        private string _password = "";
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string _errorMessage = "";

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

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

                // TODO: reemplazar con un API endpoint (/login)
                await Task.Delay(1000); // Simulate network delay

                if (ValidateCredentials(Username, Password))
                {
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

        private bool ValidateCredentials(string username, string password)
        {
            // NOTE: Este return solo realiza una validacion mockup. Se debe reemplazar directamente con el backend.
            return (username == "admin" && password == "admin") ||
                   (username == "user" && password == "user");
        }
    }
}