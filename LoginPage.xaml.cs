using LernzeitApp;
using LernzeitApp_Versuch2;
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
            if (true)//DEBUG CHANGE TO email.Length >= 26 && email.Contains("@lmg.schulen-lev.de") && password.Length >= 6
            {
                email = "ddddddddd";
                string salt = email.Substring(0, 4);
                SHA256 sHA256 = SHA256.Create();
                byte[] hash_bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
                string hash = BitConverter.ToString(hash_bytes).Replace("-", "").ToLower();
                GC.Collect();
                password = "";
                //int acces = Verify(email, hash); //1 = student 2 = teacher 3 = wrong pwd -1 = error
                int acces = 1; //DEBUG
                if (acces == -1)
                {
                    Exception ex = new Exception("Data integrity compromised!");
                    TriggerError(ex);
                }
                if (acces == 1)
                {
                    Debug.WriteLine("stage");
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
        int Verify(string username, string password)
        {
            try
            {
                TcpClient client = new TcpClient();
                LernzeitApp_Versuch2.AppInfo appInfo = new LernzeitApp_Versuch2.AppInfo();
                client.Connect(appInfo.ServerIP, appInfo.ServerPort);
                NetworkStream stream = client.GetStream();
                byte[] verify_message = Encoding.UTF8.GetBytes($"login\r\n{username}\r\n{password}");
                stream.Write(verify_message, 0, verify_message.Length);
                //Read
                byte[] buffer = new byte[1024];
                int bytesread = 0;
                while ((bytesread = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string response_encoded = Encoding.UTF8.GetString(buffer);
                    string[] content = response_encoded.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    if (content[0] == "login")
                    {
                        if (content[1] == "0")
                        {
                            return 0;
                        }
                        else if (content[1] == "1")
                        {
                            return 1;
                        }
                        else if (content[1] == "2")
                        {
                            return 2;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }

                }

            }
            catch (Exception ex)
            {
                TriggerError(ex);
                return -1;
            }
            return -1;
        }
        private async void TriggerError(object exception)
        {
            await Navigation.PushAsync(new ErrorPage(exception));
        }
        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}