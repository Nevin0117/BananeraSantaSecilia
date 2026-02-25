using SantaSecilia.ViewModels;

namespace SantaSecilia.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//Home");
}
