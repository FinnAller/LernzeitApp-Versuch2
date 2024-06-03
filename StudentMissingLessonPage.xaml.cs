namespace LernzeitApp_Versuch2;

public partial class StudentMissingLessonPage : ContentPage
{
	public StudentMissingLessonPage()
	{
		InitializeComponent();
	}
	private async void OnMenuClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new StudentMenuPage());
	}
}