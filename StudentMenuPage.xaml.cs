namespace LernzeitApp_Versuch2;

public partial class StudentMenuPage : ContentPage
{
    LernzeitApp_Versuch2.AppInfo info = new AppInfo();
    public StudentMenuPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		string path = info.LoginPath;
		if (File.Exists(path))
		{
			string[] content = File.ReadAllLines(path);
            CurrentEmailLabel.Text = $"{content[0]}";
            CurrentVersionLabel.Text = $"Version {info.Version}";
        }
		else
		{
			Exception exception = new Exception("Critical file \"AppDomain.CurrentDomain.BaseDirectory\\login.dat\" not found!");
			TriggerError(exception);
		}
    }
    private async void OnOverviewClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new StudentHomePage());
	}
	private async void OnInfoClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new StudentInfoPage());
	}
	int clicked = 0;
	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		if (clicked == 1)
		{
			string path = info.LoginPath;
			if (File.Exists(path))
			{
				File.Delete(path);
                if (File.Exists(path))
                {
                    string[] content = File.ReadAllLines(path);
                    CurrentEmailLabel.Text = $"{content[0]}";
                    CurrentVersionLabel.Text = $"{info.Version}";
                }
            }
			else
			{
				Exception exception = new Exception("Critical file not found!");
				TriggerError(exception);
			}
            clicked++;
			Application.Current.Quit();
        }
		else if(clicked == 0)
		{
			LogoutBtn.Text = "Bestätigen";
			LogoutBtn.BackgroundColor = Colors.Red;
			clicked++;
		}
		else
		{
            Exception exception = new Exception("Unknown Error\r\nat StudentMenuPage.xaml.cs\r\nat OnLogoutClicked(object, EventArgs)\r\nat clicked.Value");
            TriggerError(exception);
        }
	}
	private async void OnSupportClicked(object sender, EventArgs e)
	{
		//
	}
	private async void OnFeedbackClicked(object sender, EventArgs e)
	{
		//
	}
	private async void OnMissingLessonsClicked(object sender, EventArgs e)
	{
		Navigation.PushModalAsync(new StudentMissingLessonPage());
	}
    private async void TriggerError(object exception)
    {
        await Navigation.PushModalAsync(new ErrorPage(exception));
    }
}