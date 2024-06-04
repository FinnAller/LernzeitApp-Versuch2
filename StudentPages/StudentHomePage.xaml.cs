using System.Net.Sockets;
using System.Text;
using LernzeitApp_Versuch2.StudentPages;

namespace LernzeitApp_Versuch2;

public partial class StudentHomePage : ContentPage
{
    public StudentHomePage()
    {
        InitializeComponent();
        BindingContext = new HomePageViewModel();
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