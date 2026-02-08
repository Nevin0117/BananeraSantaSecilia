namespace SantaSecilia.ViewModels;

public class BoletaSemanalViewModel : ContentPage
{
	public BoletaSemanalViewModel()
	{
		Content = new VerticalStackLayout
		{
			Children = {
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}