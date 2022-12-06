using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class DataAdminViewModel : ViewModelBase
    {
        MainViewModel mv;
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        //Расписание

        public ObservableCollection<Timetable> timetables = new();
        public ObservableCollection<Timetable> Timetables
        {
            get => timetables;
            set => this.RaiseAndSetIfChanged(ref timetables, value);
        }

        ObservableCollection<int?> doctorIds = new();
        public ObservableCollection<int?> DoctorIds
        {
            get => doctorIds;
            set => this.RaiseAndSetIfChanged(ref doctorIds, value);
        }

        int selectedDoctorId;
        public int SelectedDoctorId
        {
            get => selectedDoctorId;
            set => this.RaiseAndSetIfChanged(ref selectedDoctorId, value);
        }

        int? doctorId;
        public int? DoctorId
        {
            get => doctorId;
            set => this.RaiseAndSetIfChanged(ref doctorId, value);
        }

        int? numberOffice;
        public int? NumberOffice
        {
            get => numberOffice;
            set => this.RaiseAndSetIfChanged(ref numberOffice, value);
        }

        string? days;
        public string? Days
        {
            get => days;
            set => this.RaiseAndSetIfChanged(ref days, value);
        }

        string? hours;
        public string? Hours
        {
            get => hours;
            set => this.RaiseAndSetIfChanged(ref hours, value);
        }

        public async void UpdateTimetables()
        {
            MV.VisibleLoad = 100;
            foreach (var timetable in Timetables)
            {
                await AsyncUpdate($"https://localhost:7242/api/Timetable/Update/{timetable.Id}", timetable);
            }
            MV.VisibleLoad = 0;
        }

        public async void DeleteTimetable()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Timetable>($"https://localhost:7242/api/Timetable/Delete/{DoctorId}");
            Timetables.Remove(Timetables.First(x => x.Id == DoctorId));
            DoctorIds.Add(DoctorId);
            MV.VisibleLoad = 0;
        }

        public async void AddTimetable()
        {
            MV.VisibleLoad = 100;
            await AsyncAdd("https://localhost:7242/api/Timetable/Add", (new Timetable(SelectedDoctorId, Days, Hours, NumberOffice)));
            Timetables.Add(new Timetable(SelectedDoctorId, Days, Hours, NumberOffice));
            DoctorIds.Remove(DoctorIds.First(x => x == SelectedDoctorId));
            MV.VisibleLoad = 0;
        }

        //Больничные листы

        ObservableCollection<SickLeave> sickLeaves = new();
        public ObservableCollection<SickLeave> SickLeaves
        {
            get => sickLeaves;
            set => this.RaiseAndSetIfChanged(ref sickLeaves, value);
        }

        public async void UpdateStatuses()
        {
            MV.VisibleLoad = 100;
            foreach (var sickLeave in SickLeaves)
            {
                if (sickLeave.Status == "открыт" && sickLeave.Open < DateTime.Now)
                {
                    sickLeave.Status = "закрыт";
                    await AsyncUpdate($"https://localhost:7242/api/SickLeave/Update/{sickLeave.Id}", sickLeave);
                }
            }          
            SickLeaves.Clear();
            var sickLeaves = await AsyncGetAll<SickLeave>("https://localhost:7242/api/SickLeave/GetAll");
            foreach (var sickLeave in sickLeaves) SickLeaves.Add(sickLeave);
            MV.VisibleLoad = 0;
        }

        //Приёмы

        ObservableCollection<Visit> visits = new();
        public ObservableCollection<Visit> Visits
        {
            get => visits;
            set => this.RaiseAndSetIfChanged(ref visits, value);
        }

        int? numberVisit;
        public int? NumberVisit
        {
            get => numberVisit;
            set => this.RaiseAndSetIfChanged(ref numberVisit, value);
        }

        public async void DeleteVisit()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Visit>($"https://localhost:7242/api/Visit/Delete/{NumberVisit}");
            Visits.Remove(Visits.First(x => x.Id == NumberVisit));
            MV.VisibleLoad = 0;
        }

        //Пациенты

        ObservableCollection<Patient> patients = new();
        public ObservableCollection<Patient> Patients
        {
            get => patients;
            set => this.RaiseAndSetIfChanged(ref patients, value);
        }

        int? numberCard;
        public int? NumberCard
        {
            get => numberCard;
            set => this.RaiseAndSetIfChanged(ref numberCard, value);
        }

        public async void DeletePatient()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Patient>($"https://localhost:7242/api/Patient/Delete/{NumberCard}");
            Patients.Remove(Patients.First(x => x.Id == NumberCard));
            MV.VisibleLoad = 0;
        }

        public DataAdminViewModel(MainViewModel mv)
        {
            MV = mv; MV.VisibleLoad = 0;
            Initialize();
        }
        async void Initialize()
        {
            MV.VisibleLoad = 100;
            var timetables = await AsyncGetAll<Timetable>("https://localhost:7242/api/Timetable/GetAll");
            var sickLeaves = await AsyncGetAll<SickLeave>("https://localhost:7242/api/SickLeave/GetAll");
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            foreach (var timetable in timetables) Timetables.Add(timetable);
            var freeDoctors = doctors.Where(x => timetables.All(y => x.Id != y.Id));
            foreach (var doctor in freeDoctors) DoctorIds.Add(doctor.Id);
            foreach (var sickLeave in sickLeaves) SickLeaves.Add(sickLeave);
            foreach (var visit in visits) Visits.Add(visit);
            foreach (var patient in patients) Patients.Add(patient);
            MV.VisibleLoad = 0;
        }

        public void Logout()
        {
            MV.VisibleLoad = 100;
            MV.Login();
        }
    }
}
