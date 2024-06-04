namespace LernzeitApp_Versuch2;

public partial class StudentInfoPage : ContentPage
{
	public StudentInfoPage()
	{
		InitializeComponent();
	}
	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new StudentMenuPage());
	}
}