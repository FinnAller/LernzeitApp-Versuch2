using System.Net.Sockets;
using System.Text;

namespace LernzeitApp_Versuch2;

public partial class StudentHomePage : ContentPage
{
    public StudentHomePage()
    {
        InitializeComponent();
        BindingContext = new HomePageViewModel();
        var ErrorDetection = new HomePageViewModel();
        for(int i = 0; i < ErrorDetection.YourEventList.Count; i++)
        {
            if (ErrorDetection.YourEventList[i].Name == "Error")
            {
                Exception exception = new Exception("Failed to receive critical data!");
                TriggerError(exception);
            }
        }
        

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
    }
    private async void OnEventTapped(object sender, EventArgs e)
    {
        var tappedEvent = (sender as VisualElement).BindingContext as Ereigniss;
        if (tappedEvent != null)
        {
            await Navigation.PushModalAsync(new StudentModulOverviewPage(tappedEvent));
        }
    }
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new StudentMenuPage());
    }
    public async void TriggerError(object exception)
    {
        await Navigation.PushModalAsync(new ErrorPage(exception));
    }
}