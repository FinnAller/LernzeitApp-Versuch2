using System.Net.Sockets;
using System.Text;

namespace LernzeitApp_Versuch2;

public partial class StudentModulOverviewPage : ContentPage
{
    Ereigniss Ereigniss { get; set; }
    public StudentModulOverviewPage(Ereigniss selectedEreigniss)
    {
        Ereigniss = selectedEreigniss;
        InitializeComponent();
        string event_name = selectedEreigniss.Name;
        string event_starttime = $"Zeitraum: {selectedEreigniss.StartTime} - {selectedEreigniss.EndTime}";
        string event_location = $"Ort: {selectedEreigniss.Location}";
        string event_slots = $"Freie Plätze: {selectedEreigniss.FreeSlots} von {selectedEreigniss.MaxSlots}";
        EventNameLabel.Text = event_name;
        EventBeginningLabel.Text = event_starttime;
        EventLocationLabel.Text = event_location;
        EventSlotsLabel.Text = event_slots;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new StudentHomePage());
    }
    private async void OnParticipateClicked(object sender, EventArgs e)
    {
        int EventID = Ereigniss.Id;
        try
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 33533);
            NetworkStream stream = client.GetStream();
            byte[] message = Encoding.UTF8.GetBytes($"register\r\n{EventID}\r\n");//IMPROVE, USER VERIFICATION NEEDED!
            //stream.Write(message, 0, message.Length);
            Thread.Sleep(300);
            byte[] buffer = new byte[1024];
            int bytesread = stream.Read(buffer, 0, buffer.Length);
            string received = Encoding.UTF8.GetString(buffer, 0, bytesread);
            string[] content = received.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (content[0] == "1")
            {
                ButtonError.TextColor = Color.Parse("green");
                ButtonError.Text = "Erfolg!";
            }
            else if (content[0] == "0")
            {
                ButtonError.Text = "Keine freien Plätze mehr!";
            }
        }
        catch (Exception ex)
        {
            TriggerError(ex);
        }
    }
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new StudentInfoPage());
    }
    private async void TriggerError(object exception)
    {
        await Navigation.PushModalAsync(new ErrorPage(exception));
    }
}