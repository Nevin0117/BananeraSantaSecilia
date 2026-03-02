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

        [NotifyPropertyChangedFor(nameof(HasError))]

        private string _errorMessage = "";



        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);



        public LoginViewModel(AuthService authService)

        {

            _authService = authService;

        }



        [RelayCommand(CanExecute = nameof(CanLogin))]

        private async Task LoginAsync()

        {

            try

            {

                IsBusy = true;

                LoginCommand.NotifyCanExecuteChanged();

                ErrorMessage = ""; // Limpiamos cualquier error previo



                if (string.IsNullOrWhiteSpace(Username))

                {

                    MostrarErrorTemporal("Por favor ingrese un usuario");

                    return;

                }



                if (string.IsNullOrWhiteSpace(Password))

                {

                    MostrarErrorTemporal("Por favor ingrese una contraseña");

                    return;

                }



                var user = await _authService.ValidateCredentialsAsync(Username, Password);



                if (user != null)

                {

                    await _authService.SetCurrentUserAsync(user.Id);

                    await Shell.Current.GoToAsync("//Home");

                }

                else

                {

                    MostrarErrorTemporal("Credenciales inválidas");

                }

            }

            catch (Exception ex)

            {

                MostrarErrorTemporal($"Error al iniciar sesión: {ex.Message}");

            }

            finally

            {

                IsBusy = false;

                LoginCommand.NotifyCanExecuteChanged();

            }

        }



        private bool CanLogin() => !IsBusy;



        // --- NUEVO MÉTODO PARA EL TEMPORIZADOR ---

        private void MostrarErrorTemporal(string mensaje)

        {

            ErrorMessage = mensaje;



            // Ejecutamos el temporizador sin bloquear la aplicación

            Task.Run(async () =>

            {

                await Task.Delay(3000);



                // Actualizamos la interfaz de usuario en el hilo principal

                MainThread.BeginInvokeOnMainThread(() =>

                {

                    // Limpiamos solo si el mensaje no ha cambiado por otro nuevo error

                    if (ErrorMessage == mensaje)

                    {

                        ErrorMessage = "";

                    }

                });

            });

        }

    }

}