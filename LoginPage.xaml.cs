namespace LernzeitApp_Versuch2;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new MainPage());
	}
}