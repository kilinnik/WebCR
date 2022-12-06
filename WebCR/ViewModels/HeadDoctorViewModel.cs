using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class HeadDoctorViewModel : ViewModelBase
    {
        MainViewModel mv;      
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        public ObservableCollection<Doctor> SearchResults { get; } = new();

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private bool showTimetable;
        public bool ShowTimetable
        {
            get => showTimetable;
            set => this.RaiseAndSetIfChanged(ref showTimetable, value);
        }

        private bool showDeleteButton;
        public bool ShowDeleteButton
        {
            get => showDeleteButton;
            set => this.RaiseAndSetIfChanged(ref showDeleteButton, value);
        }

        public async void DeleteDoctor()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Timetable>($"https://localhost:7242/api/Timetable/Delete/{SearchResults.Last().Id}");
            await AsyncDelete<Timetable>($"https://localhost:7242/api/Doctor/Delete/{SearchResults.Last().Id}");
            SearchResults.Clear();
            MV.VisibleLoad = 0;
        }

        private async void DoSearch(string s)
        {
            MV.VisibleLoad = 100;
            SearchResults.Clear();
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            foreach (var doctor in doctors)
            {
                string name = $"{doctor.Surname} {doctor.Name} {doctor.Patronymic}";
                if (s != null && (name.ToLower().Contains(s.ToLower())))
                {
                    SearchResults.Add(new Doctor(doctor.Id, doctor.Surname, doctor.Name, doctor.Patronymic, doctor.HireDate, doctor.Experience, doctor.Adress, doctor.Speciality, doctor.AreaNumber, doctor.Phone));
                    ShowTimetable = true;
                }
            }
            if (s == "") ShowTimetable = false;
            if (SearchResults.Count == 1) ShowDeleteButton = true;
            else ShowDeleteButton = false;
            MV.VisibleLoad = 0;
        }

        public HeadDoctorViewModel(MainViewModel mv)
        {
            MV = mv; MV.VisibleLoad = 0;
            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);
        }

        public void Logout()
        {
            MV.VisibleLoad = 100;
            MV.Login();
        }

        private bool successAddDoctor;
        public bool SuccessAddDoctor
        {
            get => successAddDoctor;
            set => this.RaiseAndSetIfChanged(ref successAddDoctor, value);
        }

        string? surname;
        public string? Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }

        string? name;
        public string? Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string? patronymic;
        public string? Patronymic
        {
            get => patronymic;
            private set => this.RaiseAndSetIfChanged(ref patronymic, value);
        }

        long? phone;
        public long? Phone
        {
            get => phone;
            private set => this.RaiseAndSetIfChanged(ref phone, value);
        }

        string? hireDate;
        public string? HireDate
        {
            get => hireDate;
            private set => this.RaiseAndSetIfChanged(ref hireDate, value);
        }

        int? seniority;
        public int? Seniority
        {
            get => seniority;
            private set => this.RaiseAndSetIfChanged(ref seniority, value);
        }

        string? adress;
        public string? Adress
        {
            get => adress;
            private set => this.RaiseAndSetIfChanged(ref adress, value);
        }

        string? speciality;
        public string? Speciality
        {
            get => speciality;
            private set => this.RaiseAndSetIfChanged(ref speciality, value);
        }

        int? areaNumber;
        public int? AreaNumber
        {
            get => areaNumber;
            private set => this.RaiseAndSetIfChanged(ref areaNumber, value);
        }

        public async void AddDoctorButton()
        {
            MV.VisibleLoad = 100;
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var id = doctors.Last().Id + 1;
            await AsyncAdd("https://localhost:7242/api/Doctor/Add", new Doctor(id, Surname, Name, Patronymic, DateTime.Parse(HireDate), Seniority, Adress, Speciality, AreaNumber, Phone));
            SuccessAddDoctor = true;
            MV.VisibleLoad = 0;
        }
    }
}
