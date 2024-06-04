using LernzeitApp_Versuch2;
using LernzeitApp_Versuch2.CorePages;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace LernzeitApp_Versuch2
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var viewModel = (LoginPageViewModel)BindingContext;
            string email = viewModel.InputEmail;
            string password = viewModel.InputPassword;
            if(email == null)
            {
                email = System.String.Empty;
            }
            if(password == null)
            {
                password = System.String.Empty;
            }
            if (email.Length >= 26 && email.Contains("@lmg.schulen-lev.de") && password.Length >= 6)//DEBUG CHANGE TO true
            {
                string salt = email.Substring(0, 4);
                SHA256 sHA256 = SHA256.Create();
                byte[] hash_bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
                string hash = BitConverter.ToString(hash_bytes).Replace("-", "").ToLower();
                GC.Collect();
                password = "";
                int acces = await VerifyAsync(email, hash); //1 = student 2 = teacher 0 = wrong pwd -1 = error
                //int acces = 1; //DEBUG
                if (acces == -1)
                {
                    Exception ex = new Exception("Data integrity compromised!");
                    TriggerError(ex);
                }
                if (acces == 1)
                {
                    string[] logindata = new string[]
                    {
                        email,
                        password,
                        DateTime.Now.ToString()
                    };
                    LernzeitApp_Versuch2.AppInfo info = new AppInfo();
                    File.WriteAllLines(info.LoginPath, logindata);
                    await Navigation.PushModalAsync(new StudentHomePage());
                }
                else if (acces == 2)
                {
                    //await Navigation.PushModalAsync(new TeacherHomePage());//MUSS ERSTELLT WERDEN!
                }
                else if (acces == 0)
                {
                    viewModel.ErrorMessage = "Das Password ist falsch!";
                }
                else
                {
                    Exception ex = new Exception("Data intergrity compromised!");
                    TriggerError(ex);
                }
            }
            else
            {
                viewModel.ErrorMessage = "Die eingegebenen Daten sind nicht korrekt!";
            }
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
        private async void TriggerError(object exception)
        {
            await Navigation.PushModalAsync(new ErrorPage(exception));
        }
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}