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
            //await Navigation.PushModalAsync(); //LoginPage.xaml
        }
        private async void OnInfoClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InfoPage()); //InfoPage.xaml
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            string logindata = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\", "login.dat");
        }
    }
    public class AppInfo
    {
        public string Version { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public AppInfo()
        {
            Version = "0.0.2";
            ServerIP = "127.0.0.1";
            ServerPort = 33533;
        }
    }
}
