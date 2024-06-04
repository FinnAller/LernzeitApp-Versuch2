using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LernzeitApp_Versuch2.StudentPages
{
    public class Fehlstunde : INotifyPropertyChanged
    {
        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        private string _excused;
        public string Excused
        {
            get { return _excused; }
            set { _excused = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class StudentMissingLessonViewModel
    {
        public List<Fehlstunde> MissingLessons { get; set; }
        public StudentMissingLessonViewModel()
        {
            List<Fehlstunde> fehlstunden = GetMissingLessons();
            MissingLessons = fehlstunden;
        }
        private List<Fehlstunde> GetMissingLessons()
        {
            //DEBUG
            List<Fehlstunde> fehlstunden = new List<Fehlstunde>()
            {
                new Fehlstunde { Date = "12.12.2023", Excused = "Nein"},
                new Fehlstunde { Date = "17.2.2024", Excused = "Ja"}
            };
            //DEBUG END
            return fehlstunden;
        }
    }
}
