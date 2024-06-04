using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LernzeitApp_Versuch2.StudentPages
{
    public class Ereigniss : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
        }
        private string _endTime;
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(); }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; OnPropertyChanged(); }
        }
        private string _freeSlots;
        public string FreeSlots
        {
            get { return _freeSlots; }
            set { _freeSlots = value; OnPropertyChanged(); }
        }
        private string _maxSlots;
        public string MaxSlots
        {
            get { return _maxSlots; }
            set { _maxSlots = value; OnPropertyChanged(); }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class HomePageViewModel :INotifyPropertyChanged
    {
        private List<Ereigniss> _yourEventList;
        public List<Ereigniss> YourEventList
        {
            get => _yourEventList;
            set
            {
                _yourEventList = value;
                OnPropertyChanged();
            }
        }

        public HomePageViewModel()
        {
            
        }

        public async Task InitializeAsync()
        {
            await Task.Delay(1000);
            YourEventList = await GetModules();
            if(YourEventList != null && YourEventList.Count > 0)
            {
                Debug.WriteLine("Modules succesfully loaded!");
            }
            else
            {
                Debug.WriteLine("ERROR!");
            }
        }

        private async Task<List<Ereigniss>> GetModules()
        {
            List<Ereigniss> ereignisse = new List<Ereigniss>();
            LernzeitApp_Versuch2.AppInfo appInfo = new LernzeitApp_Versuch2.AppInfo();
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(appInfo.ServerIP, appInfo.ServerPort);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] message = Encoding.UTF8.GetBytes("getmods");
                        await stream.WriteAsync(message, 0, message.Length);

                        // Read
                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string responseEncoded = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] response = responseEncoded.Split(new string[] { "\r" }, StringSplitOptions.None);
                        if (response[0] == "getmods")
                        {
                            for (int i = 1; i < response.Length; i++)
                            {
                                Ereigniss current = new Ereigniss();
                                string[] parameters = response[i].Split(new string[] { "\n" }, StringSplitOptions.None);
                                current.Name = parameters[0];
                                current.StartTime = parameters[1];
                                current.EndTime = parameters[2];
                                current.Location = parameters[3];
                                current.FreeSlots = parameters[4];
                                current.MaxSlots = parameters[5];
                                ereignisse.Add(current);
                            }
                        }
                        else
                        {
                            ereignisse.Add(new Ereigniss { Name = "Error" });
                        }
                        // Debugging: Ausgabe der geladenen Ereignisse
                        foreach (var ereignis in ereignisse)
                        {
                            Debug.WriteLine($"Loaded event: {ereignis.Name}, {ereignis.StartTime}, {ereignis.EndTime}");
                        }

                        stream.Close();
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ereignisse.Add(new Ereigniss { Name = "Error" });
            }
            return ereignisse;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
