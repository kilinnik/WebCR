using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class PatientViewModel : ViewModelBase
    {
        MainViewModel mw;
        public MainViewModel MV
        {
            get => mw;
            private set => this.RaiseAndSetIfChanged(ref mw, value);
        }

        private int visibleLoad;
        public int VisibleLoad
        {
            get => visibleLoad;
            set => this.RaiseAndSetIfChanged(ref visibleLoad, value);
        }

        int? CardNumber { get; set; } //номер карты текущего пациента

        //Запись на приём 
        //словарь для сопоставления выбранного дня приёма с расписанием врача
        readonly Dictionary<string, string> DaysWeek = new() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };

        ObservableCollection<string> specialities = new(); //список специальностей
        public ObservableCollection<string> Specialities
        {
            get => specialities;
            set => this.RaiseAndSetIfChanged(ref specialities, value);
        }

        string? selectedSpeciality;
        public string? SelectedSpeciality
        {
            get => selectedSpeciality;
            set => this.RaiseAndSetIfChanged(ref selectedSpeciality, value);
        }

        bool visibleCard;
        public bool VisibleCard
        {
            get => visibleCard;
            set => this.RaiseAndSetIfChanged(ref visibleCard, value);
        }

        bool enabledHours; //для переключения часов
        public bool EnabledHours
        {
            get => enabledHours;
            set => this.RaiseAndSetIfChanged(ref enabledHours, value);
        }

        bool visibleHours;
        public bool VisibleHours
        {
            get => visibleHours;
            set => this.RaiseAndSetIfChanged(ref visibleHours, value);
        }

        bool onButton; //вкл/откл кнопки записи
        public bool OnButton
        {
            get => onButton;
            set => this.RaiseAndSetIfChanged(ref onButton, value);
        }

        string? labelVisitDate;
        public string? LabelVisitDate
        {
            get => labelVisitDate;
            set => this.RaiseAndSetIfChanged(ref labelVisitDate, value);
        }

        string? textTime;
        public string? TextTime
        {
            get => textTime;
            set => this.RaiseAndSetIfChanged(ref textTime, value);
        }

        string? labelSpeciality;
        public string? LabelSpeciality
        {
            get => labelSpeciality;
            set => this.RaiseAndSetIfChanged(ref labelSpeciality, value);
        }

        string? visitDate;
        public string? VisitDate
        {
            get => visitDate;
            private set => this.RaiseAndSetIfChanged(ref visitDate, value);
        }

        string? icon; //смена иконки переключателя часов
        public string? Icon
        {
            get => icon;
            private set => this.RaiseAndSetIfChanged(ref icon, value);
        }

        int hours;
        public int Hours
        {
            get => hours;
            set => this.RaiseAndSetIfChanged(ref hours, value);
        }

        int minutes;
        public int Minutes
        {
            get => minutes;
            set => this.RaiseAndSetIfChanged(ref minutes, value);
        }

        string? wrongSpecialities;
        public string? WrongSpecialities
        {
            get => wrongSpecialities;
            private set => this.RaiseAndSetIfChanged(ref wrongSpecialities, value);
        }

        string? wrongDate;
        public string? WrongDate
        {
            get => wrongDate;
            private set => this.RaiseAndSetIfChanged(ref wrongDate, value);
        }

        string? wrongTime;
        public string? WrongTime
        {
            get => wrongTime;
            private set => this.RaiseAndSetIfChanged(ref wrongTime, value);
        }

        string? textAppointment;
        public string? TextAppointment
        {
            get => textAppointment;
            private set => this.RaiseAndSetIfChanged(ref textAppointment, value);
        }

        bool textAppointmentVisible;
        public bool TextAppointmentVisible
        {
            get => textAppointmentVisible;
            set => this.RaiseAndSetIfChanged(ref textAppointmentVisible, value);
        }

        bool chooseVisible;
        public bool ChooseVisible
        {
            get => chooseVisible;
            set => this.RaiseAndSetIfChanged(ref chooseVisible, value);
        }

        public void Appointment()
        {
            WrongSpecialities = null; WrongDate = null; WrongTime = null;
            if (SelectedSpeciality == null) WrongSpecialities = "выберете специальность";
            if (VisitDate == null) WrongDate = "выберете дату";
            else if (Convert.ToDateTime(VisitDate) <= DateTime.Today) WrongDate = "некорректная дата";
            if (Hours < 8 || Hours >= 20) WrongTime = "некорректное время";
            if (WrongSpecialities == null && WrongDate == null && WrongTime == null) CheckData();
        }

        public async void CheckData()
        {
            VisibleLoad = 100;
            ChooseVisible = false; VisibleCard = true;
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var timetables = await AsyncGetAll<Timetable>("https://localhost:7242/api/Timetable/GetAll");
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var doctor = doctors.First(x => x.AreaNumber == patients.First(y => y.Id== CardNumber).AreaNumber && x.Speciality == SelectedSpeciality);
            var doctorTimetable = timetables.First(x => x.Id == doctor.Id);
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            if (DaysWeek[doctorTimetable.VisitDays].Contains(date.DayOfWeek.ToString()) && CheckTime(doctorTimetable, Hours))
            {
                if (Minutes % 30 == 0 && !visits.Where(x => x.DoctorId == doctor.Id).Any(x => x.VisitDate == date))
                {
                    TextAppointmentVisible = true; TextAppointment = "Вы записаны на приём";
                    date = date.Date + new TimeSpan(Hours, Minutes, 0);
                    int visitNumber = visits.OrderBy(x => x.Id).Last().Id + 1;
                    var patientVisit = new Visit(visitNumber, doctor.Id, CardNumber, date);
                    var patientComplaint = new Complaints(visitNumber, null, null);
                    await AsyncAdd("https://localhost:7242/api/Visit/Add", patientVisit);
                    await AsyncAdd("https://localhost:7242/api/Complaints/Add", patientComplaint);
                    FillGrid();
                }
                else
                {
                    int minutes1 = Minutes; int hours1 = Hours;
                    if (minutes1 > 15 && minutes1 < 45) minutes1 = 30;
                    else if (minutes1 <= 15) minutes1 = 0;
                    else { minutes1 = 0; hours1++; }
                    int minutes2 = minutes1; int hours2 = hours1;
                    for (int i = 0; ; i += 30)
                    {
                        minutes1 += i; minutes2 -= i;
                        if (CheckTime(doctorTimetable, hours1 + minutes1 / 60))
                        {
                            date = date.Date + new TimeSpan(hours1 + minutes1 / 60, minutes1 % 60, 0);
                            if (!visits.Where(x => x.DoctorId == doctor.Id).Any(x => x.VisitDate == date))
                            {
                                TextAppointment = $"Время {Hours}:{Minutes} занято, ближайшее свободное {hours1 + minutes1 / 60}:{minutes1 % 60}. Записаться на это время?";
                                Hours = hours1 + minutes1 / 60; Minutes = minutes1 % 60; ChooseVisible = true; TextAppointmentVisible = true; OnButton = false;
                                break;
                            }
                        }
                        if (CheckTime(doctorTimetable, hours2 + (int)Math.Floor((double)minutes2 / 60)))
                        {
                            date = date.Date + new TimeSpan(hours2 + (int)Math.Floor((double)minutes2 / 60), Math.Abs(minutes2 % 60), 0);
                            if (!visits.Where(x => x.DoctorId == doctor.Id).Any(x => x.VisitDate== date))
                            {
                                TextAppointment = $"Время {Hours}:{Minutes} занято, ближайшее свободное время {hours2 + (int)Math.Floor((double)minutes2 / 60)}:{Math.Abs(minutes2 % 60)}. Записаться на это время?";
                                Hours = hours2 + (int)Math.Floor((double)minutes2 / 60); Minutes = Math.Abs(minutes2 % 60); ChooseVisible = true; TextAppointmentVisible = true; OnButton = false;
                                break;
                            }
                        }
                        if (!CheckTime(doctorTimetable, hours1 + minutes1 / 60) && !CheckTime(doctorTimetable, hours2 + minutes2 / 60)) break;
                    }
                }
            }
            else
            {
                TextAppointmentVisible = true; TextAppointment = "В это время врач не принимает";
            }
            VisibleLoad = 0;
        }

        public bool CheckTime(Timetable timetable, int hours)
        {
            bool result = false;
            var time = timetable.VisitHours.Split(",");
            foreach (var t in time)
            {
                var temp = t.Split("-");
                if (Convert.ToUInt32(temp[0][..2]) <= hours && Convert.ToUInt32(temp[1][..2]) > hours) result = true;
            }
            return result;
        }

        public async void Accept()
        {
            VisibleLoad = 100;
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var complaints = await AsyncGetAll<Complaints>("https://localhost:7242/api/Complaints/GetAll");
            TextAppointment = "Вы записаны на приём"; ChooseVisible = false; OnButton = true;
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            int visitNumber = visits.OrderBy(x => x.Id).Last().Id + 1;
            var patientVisit = new Visit(visitNumber, doctors.First(x => x.AreaNumber == patients.First(y => y.Id == CardNumber).AreaNumber && x.Speciality == SelectedSpeciality).Id, CardNumber, date);
            var patientComplaint = new Complaints(visitNumber, null, null);
            await AsyncAdd("https://localhost:7242/api/Visit/Add", patientVisit);
            await AsyncAdd("https://localhost:7242/api/Complaints/Add", patientComplaint);
            FillGrid();
            VisibleLoad = 0;
        }

        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }

        public void Switch() //переключение часов
        {
            if (Icon == "ToggleSwitchOffOutline")
            {
                Icon = "ToggleSwitchOutline"; EnabledHours = false; VisibleHours = false;
            }
            else
            {
                Icon = "ToggleSwitchOffOutline"; EnabledHours = true; VisibleHours = true;
            }
        }

        //Карта пациента
        public ObservableCollection<DataVisit> dataVisits; //список визитов пациента
        public ObservableCollection<DataVisit> DataVisits
        {
            get => dataVisits;
            private set => this.RaiseAndSetIfChanged(ref dataVisits, value);
        }

        bool enabledUserData; //редактирование данных о пациенте
        public bool EnabledUserData
        {
            get => enabledUserData;
            set => this.RaiseAndSetIfChanged(ref enabledUserData, value);
        }

        string? textButtonChangeUserData;
        public string? TextButtonChangeUserData
        {
            get => textButtonChangeUserData;
            private set => this.RaiseAndSetIfChanged(ref textButtonChangeUserData, value);
        }

        string surname;
        public string Surname
        {
            get => surname;
            private set => this.RaiseAndSetIfChanged(ref surname, value);
        }

        string name;
        public string Name
        {
            get => name;
            private set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string patronymic;
        public string Patronymic
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

        string dateOfBirthday;
        public string DateOfBirthday
        {
            get => dateOfBirthday;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirthday, value);
        }

        string adress;
        public string Adress
        {
            get => adress;
            private set => this.RaiseAndSetIfChanged(ref adress, value);
        }

        string login;
        public string Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string password;
        public string Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public async void FillGrid()
        {
            var visits =await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var timetables = await AsyncGetAll<Timetable>("https://localhost:7242/api/Timetable/GetAll");
            var dataVisit = new List<DataVisit>();
            var patientVisits = visits.Where(x => x.VisitDate > DateTime.Now && x.CardNumber == CardNumber).OrderBy(x => x.VisitDate).ToArray();
            foreach (var patinetVisit in patientVisits)
            {
                var doctor = doctors.First(x => x.Id == patinetVisit.DoctorId);
                string name = $"{doctor.Surname} {doctor.Name[0]}.{doctor.Patronymic[0]}.";
                dataVisit.Add(new DataVisit() { DateTime = patinetVisit.VisitDate.ToString("dd/MM/yyyy HH:mm"), Speciality = doctor.Speciality, Name = name, Office = (int)timetables.First(x => x.Id == doctor.Id).OfficeNumber });
            }
            DataVisits = new ObservableCollection<DataVisit>(dataVisit);
        }

        public async void ButtonChangeUserData()
        {
            VisibleLoad = 100;
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");
            if (TextButtonChangeUserData == "Изменить данные")
            {
                TextButtonChangeUserData = "Сохранить данные"; EnabledUserData = true;
            }
            else
            {
                TextButtonChangeUserData = "Изменить данные"; EnabledUserData = false;
                var patient = patients.First(y => y.Id == CardNumber); var user = dataLogins.First(x => x.IdPatientOrDoctor == CardNumber && x.Role == "Пациент");
                patient.Surname = Surname; patient.Name = Name; patient.Patronymic = Patronymic; patient.Phone = Phone; patient.DateOfBirth = Convert.ToDateTime(DateOfBirthday); patient.Adress = Adress; user.Login = Login; user.Password = Password;
                await AsyncUpdate($"https://localhost:7242/api/Patient/Update/{patient.Id}", patient);
                await AsyncUpdate($"https://localhost:7242/api/Login/Update/{user.Id}", user);
            }
            VisibleLoad = 0;
        }

        //Расписание врачей
        public ObservableCollection<TimetableDoctors> SearchResults { get; } = new(); //список расписаний
                                                                                      
        private bool showTimetable;
        public bool ShowTimetable
        {
            get => showTimetable;
            set => this.RaiseAndSetIfChanged(ref showTimetable, value);
        }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private async void DoSearch(string s)
        {
            VisibleLoad = 100;
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var timetables = await AsyncGetAll<Timetable>("https://localhost:7242/api/Timetable/GetAll");
            SearchResults.Clear();
            var doctorsForTt = doctors.Where(x => x.AreaNumber == patients.First(y => y.Id == CardNumber).AreaNumber).ToArray();
            foreach (var doctor in doctorsForTt)
            {
                string name = $"{doctor.Surname} {doctor.Name[0]}.{doctor.Patronymic[0]}.";
                var timetable = timetables.FirstOrDefault(x => x.Id == doctor.Id);
                if (timetable != null && s != null && (timetable.OfficeNumber.ToString().Contains(s) || name.ToLower().Contains(s.ToLower())))
                {
                    SearchResults.Add(new TimetableDoctors() { Name = name, Speciality = doctor.Speciality, Office = (int)timetable.OfficeNumber, Days = timetable.VisitDays, Time = timetable.VisitHours });
                    ShowTimetable = true;
                }
                if (s == "") ShowTimetable = false;
            }
            VisibleLoad = 0;
        }

        public PatientViewModel(int? cardNumber, MainViewModel mv)
        {
            MV = mv; CardNumber = cardNumber; LabelSpeciality = "Специальность врача"; OnButton = true; LabelVisitDate = "Дата приёма"; Icon = "ToggleSwitchOffOutline"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма";

            Initialize();

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);
        }

        async void Initialize()
        {
            VisibleLoad = 100;
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");

            var listSpecialities = doctors.Where(x => x.AreaNumber == patients.First(y => y.Id == CardNumber).AreaNumber).Select(x => x.Speciality);
            foreach (var item in listSpecialities) Specialities.Add(item);

            var patient = patients.First(y => y.Id == CardNumber); var user = dataLogins.First(x => x.IdPatientOrDoctor == CardNumber && x.Role == "Пациент"); TextButtonChangeUserData = "Изменить данные";
            Surname = patient.Surname; Name = patient.Name; Patronymic = patient.Patronymic; Phone = patient.Phone; DateOfBirthday = patient.DateOfBirth.ToString("dd/MM/yyyy"); Adress = patient.Adress; Login = user.Login; Password = user.Password;
            FillGrid();

            VisibleLoad = 0;
        }

        public void Logout()
        {
            MV.Login();
        }
    }
}
