﻿using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LernzeitApp_Versuch2
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
        public string EndTime//Not implemented
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value; OnPropertyChanged();
            }
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
            get
            {
                return _freeSlots;
            }
            set
            {
                _freeSlots = value; OnPropertyChanged();
            }
        }
        private string _maxSlots;
        public string MaxSlots //Not implemented
        {
            get
            {
                return _maxSlots;
            }
            set
            {
                _maxSlots = value; OnPropertyChanged();
            }
        }
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class HomePageViewModel
    {
        public List<Ereigniss> YourEventList { get; set; }

        public HomePageViewModel()
        {
            List<Ereigniss> ereignisse = GetModules();
            YourEventList = ereignisse;
            /*
            YourEventList = new List<Ereigniss>
            {
                new Ereigniss { Name = "Schach-AG", StartTime = "09:00", Location = "Raum A", FreeSlots = "8", EndTime = "9:45"},
                new Ereigniss { Name = "Schule ohne Rassismus", StartTime = "10:30", Location = "Raum B", FreeSlots = "3", EndTime = "11:30"},
                new Ereigniss { Name= "Philosophie", StartTime = "15:00", Location = "Raum D", FreeSlots = "2", EndTime = "16:45"},
                new Ereigniss { Name="Mathe-Lernzeit", StartTime = "12:55", Location = "Raum A", EndTime = "14:25"},
                new Ereigniss { StartTime = "14:45", Location = "Raum D", Name = "Deutsch-Lernzeit", FreeSlots = "8", EndTime = "15:30"},
                new Ereigniss { StartTime = "12:40", Location = "Raum E", FreeSlots = "16", Name = "Latein-Lernzeit", EndTime = "13:25"},
                new Ereigniss { StartTime = "15:05", Location = "Raum A", Name = "Deutsch ?-Stunde", FreeSlots = "0", EndTime = "16:00"},
                new Ereigniss { StartTime = "17:30", Location = "Raum C", Name = "Roboter-AG", FreeSlots = "1", EndTime = "18:15"},
                new Ereigniss { StartTime = "9:00", Location = "Raum D", FreeSlots = "5", Name = "Kunst-AG", EndTime = "10:30"},
                new Ereigniss { StartTime = "15:05", Location = "Raum B", FreeSlots = "14", Name = "SV-Sitzung", EndTime = "16:50"}
            };
            */
        }
        private List<Ereigniss> GetModules()
        {
            List<Ereigniss> ereignisse = new List<Ereigniss>();
            LernzeitApp_Versuch2.AppInfo info = new AppInfo();
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(info.ServerIP, info.ServerPort);
                NetworkStream stream = client.GetStream();
                byte[] message = Encoding.UTF8.GetBytes($"getmods");
                stream.Write(message, 0, message.Length);
                byte[] responseBytes = new byte[256];
                Thread.Sleep(300);
                int bytes = stream.Read(responseBytes, 0, responseBytes.Length);
                string responsestring = Encoding.UTF8.GetString(responseBytes);
                string[] response = responsestring.Split(new string[] { "\r" }, StringSplitOptions.None);
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
                    return ereignisse;
                }
                else
                {
                    Ereigniss error_ereigniss = new Ereigniss();
                    error_ereigniss.Name = "Error";
                    ereignisse.Add(error_ereigniss);
                    return ereignisse;
                }
            }
            catch (Exception ex)
            {
                Ereigniss error_ereigniss = new Ereigniss();
                error_ereigniss.Name = "Error";
                ereignisse.Add(error_ereigniss);
                return ereignisse;
            }
        }
    }
}
