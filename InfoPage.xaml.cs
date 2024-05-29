namespace LernzeitApp_Versuch2;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
	}
	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new MainPage());
	}
}