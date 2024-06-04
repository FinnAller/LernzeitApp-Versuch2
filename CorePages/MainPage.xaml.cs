using System.Diagnostics;
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
            LernzeitApp_Versuch2.AppInfo info = new AppInfo();
            string logindatapath = info.LoginPath;
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
                            int acces = await VerifyAsync(email, hash);
                            switch (acces)
                            {
                                case 0:
                                    File.Delete(logindatapath);
                                    break;
                                case 1:
                                    await Navigation.PushModalAsync(new StudentHomePage());
                                    break;
                                case 2:
                                    //LehrerLogin Hier!
                                    break;
                                default:
                                    Exception InvalidResponse = new Exception("Server response invalid");
                                    TriggerError(InvalidResponse);
                                    break;
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
        private async Task<int> VerifyAsync(string username, string password)
        {
            try
            {
                TcpClient client = new TcpClient();
                LernzeitApp_Versuch2.AppInfo appInfo = new LernzeitApp_Versuch2.AppInfo();
                await client.ConnectAsync(appInfo.ServerIP, appInfo.ServerPort);
                NetworkStream stream = client.GetStream();
                byte[] verify_message = Encoding.UTF8.GetBytes($"login\r\n{username}\r\n{password}");
                await stream.WriteAsync(verify_message, 0, verify_message.Length);

                // Read
                byte[] buffer = new byte[1024];
                int bytesread = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response_encoded = Encoding.UTF8.GetString(buffer, 0, bytesread);
                string[] content = response_encoded.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (content.Length >= 2 && content[0] == "login")
                {
                    switch (content[1])
                    {
                        case "0":
                            Debug.WriteLine("Wrong pwd");
                            return 0;
                        case "1":
                            Debug.WriteLine("Student");
                            return 1;
                        case "2":
                            Debug.WriteLine("Teacher");
                            return 2;
                        default:
                            return -1;
                    }
                }
                else
                {
                    Debug.WriteLine("really corrupted");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                TriggerError(ex);
                return -1;
            }
        }
    }
    public class AppInfo
    {
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string LoginPath { get; set; }
        public AppInfo()
        {
            LoginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "login.dat");
            Version = "0.0.12.1";
            ServerIP = "127.0.0.1";
            ServerPort = 33533;
        }
    }
}
