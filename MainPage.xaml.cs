using System.Net.Sockets;
using System.Text;

namespace LernzeitApp_Versuch2
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoginPage()); //LoginPage.xaml
        }
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InfoPage()); //InfoPage.xaml
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            string logindatapath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\", "login.dat");
            if (File.Exists(logindatapath))
            {
                try
                {
                    string[] logindata = File.ReadAllLines(logindatapath);
                    if(logindata.Length == 3)
                    {
                        string email = logindata[0];
                        string hash = logindata[1];
                        DateTime creation = DateTime.Parse(logindata[2]);
                        TimeSpan maxtime = new TimeSpan(30, 0, 0, 0);
                        if((DateTime.Now - creation) > maxtime)
                        {
                            File.Delete(logindatapath);
                        }
                        else
                        {
                            TcpClient client = new TcpClient();
                            AppInfo appinfo = new AppInfo();
                            client.Connect(appinfo.ServerIP, appinfo.ServerPort);
                            NetworkStream stream = client.GetStream();
                            byte[] message = Encoding.UTF8.GetBytes($"verify\r\n{email}\r\n{hash}");
                            await stream.WriteAsync(message, 0, message.Length);
                            byte[] responseBytes = new byte[256];
                            int bytes = await stream.ReadAsync(responseBytes, 0, responseBytes.Length);
                            string responsestring = Encoding.UTF8.GetString(responseBytes);
                            string[] response = responsestring.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                            if(response.Length == 2 && response[0] == "verify")
                            {
                                switch (response[1])
                                {
                                    case "0":
                                        File.Delete(logindatapath);
                                        break;
                                    case "1":
                                        //SchülerLogin Hier!
                                        break;
                                    case "2":
                                        //LehrerLogin Hier!
                                        break;
                                    default:
                                        Exception InvalidResponse = new Exception("Server response invalid");
                                        TriggerError(InvalidResponse);
                                        break;
                                }
                            }
                            else
                            {
                                Exception InvalidResponse = new Exception("Server response invalid");
                                TriggerError(InvalidResponse);
                            }
                        }
                    }
                    else
                    {
                        File.Delete(logindatapath);
                    }
                    
                }
                catch (Exception ex)
                {
                    TriggerError(ex);
                }
            }
        }
        private async void TriggerError(object exception)
        {
            await Navigation.PushAsync(new ErrorPage(exception));
        }
    }
    public class AppInfo
    {
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public AppInfo()
        {
            Version = "0.0.6";
            ServerIP = "127.0.0.1";
            ServerPort = 33533;
        }
    }
}
