using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class RegistrarViewModel: ViewModelBase
    {
        MainViewModel mv;
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        private int visibleLoad;
        public int VisibleLoad
        {
            get => visibleLoad;
            set => this.RaiseAndSetIfChanged(ref visibleLoad, value);
        }

        //Добавить пациента
        //список улиц по участкам
        static List<List<string>> ListOfArea = new List<List<string>>() { new List<string> { "ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> { "ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> { "ул. Филатова", "Школьная ул." } };
       
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

        string? dateOfBirth;
        public string? DateOfBirth
        {
            get => dateOfBirth;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirth, value);
        }

        ObservableCollection<string> streets = new ObservableCollection<string>(); //список улиц
        public ObservableCollection<string> Streets
        {
            get => streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }

        string? selectedStreet;
        public string? SelectedStreet
        {
            get => selectedStreet;
            set => this.RaiseAndSetIfChanged(ref selectedStreet, value);
        }

        int? house;
        public int? House
        {
            get => house;
            set => this.RaiseAndSetIfChanged(ref house, value);
        }

        int? flat;
        public int? Flat
        {
            get => flat;
            set => this.RaiseAndSetIfChanged(ref flat, value);
        }

        int? indexCurStreet;
        public int? IndexCurStreet
        {
            get => indexCurStreet;
            set => this.RaiseAndSetIfChanged(ref indexCurStreet, value);
        }

        string? labelSurname;
        public string? LabelSurname
        {
            get => labelSurname;
            set => this.RaiseAndSetIfChanged(ref labelSurname, value);
        }

        string? labelName;
        public string? LabelName
        {
            get => labelName;
            set => this.RaiseAndSetIfChanged(ref labelName, value);
        }

        string? labelPatronymic;
        public string? LabelPatronymic
        {
            get => labelPatronymic;
            set => this.RaiseAndSetIfChanged(ref labelPatronymic, value);
        }

        string? labelPhone;
        public string? LabelPhone
        {
            get => labelPhone;
            set => this.RaiseAndSetIfChanged(ref labelPhone, value);
        }

        string? labelDateOfBirth;
        public string? LabelDateOfBirth
        {
            get => labelDateOfBirth;
            set => this.RaiseAndSetIfChanged(ref labelDateOfBirth, value);
        }

        string? labelAdress;
        public string? LabelAdress
        {
            get => labelAdress;
            set => this.RaiseAndSetIfChanged(ref labelAdress, value);
        }

        bool showFloatingWatermark;
        public bool ShowFloatingWatermark
        {
            get => showFloatingWatermark;
            private set => this.RaiseAndSetIfChanged(ref showFloatingWatermark, value);
        }

        bool showSuccessAddPatient;
        public bool ShowSuccessAddPatient
        {
            get => showSuccessAddPatient;
            private set => this.RaiseAndSetIfChanged(ref showSuccessAddPatient, value);
        }

        public void AddPatient()
        {
            //using var db = new СlinicReceptionContext();
            //LabelSurname = null; LabelName = null; LabelPatronymic = null; LabelPhone = null; LabelDateOfBirth = null; LabelAdress = null; ShowSuccessAddPatient = false;
            //if (Surname == null) LabelSurname = "Введите фамилию";
            //else if (!Surname.All(Char.IsLetter)) LabelSurname = "Фамилия содержит недопустимые символы";
            //if (Name == null) LabelName = "Введите имя";
            //else if (!Name.All(Char.IsLetter)) LabelName = "Имя содержит недопустимые символы";
            //if (Patronymic == null) LabelPatronymic = "Введите отчество";
            //else if (!Patronymic.All(Char.IsLetter)) LabelPatronymic = "Отчество содержит недопустимые символы";
            //if (Phone == null) LabelPhone = "Введите номер телефона";
            //else if (Phone.ToString()?.Length != 10) LabelPhone = "Неверный формат номера телефона";
            //if (DateOfBirth == null) LabelDateOfBirth = "Выберете дату рождения";
            //else if (Convert.ToDateTime(DateOfBirth) > DateTime.Today) LabelDateOfBirth = "Некорректная дата рождения";
            //if (SelectedStreet == null || House == null) LabelAdress = "Введите адрес";
            //if (LabelSurname == null && LabelName == null && LabelPatronymic == null && LabelPhone == null && LabelDateOfBirth == null && LabelAdress == null)
            //{              
            //    int area = IndexCurStreet < ListOfArea[0].Count ? 1 : IndexCurStreet < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
            //    string adress;
            //    if (Flat == null) adress = $"{SelectedStreet}, д. {House}";
            //    else adress = $"{SelectedStreet}, д. {House}, кв. {Flat}";
            //    var number = db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1;
            //    db.Add(new Patient(number, Surname, Name, Patronymic, (long)Phone, Convert.ToDateTime(DateOfBirth), area, adress));
            //    db.Add(new DataLogin(number.ToString(), number.ToString(), "Пациент", db.Пациент.OrderBy(x => x.Номер_карты).Last().Номер_карты + 1));
            //    db.SaveChanges();
            //    ShowSuccessAddPatient = true;
            //}
            //else ShowFloatingWatermark = true;
        }

        //Запись на приём
        //словарь для сопоставления выбранного дня приёма с расписанием врача
        Dictionary<string, string> DaysWeek = new Dictionary<string, string>() { { "Пн-Пт", "Monday, Tuesday, Wednesday, Thursday, Friday" }, { "Пн-Чт", "Monday, Tuesday, Wednesday, Thursday" }, { "Вт-Пт", "Tuesday, Wednesday, Thursday, Friday" }, { "Вт-Сб", "Tuesday, Wednesday, Thursday, Friday, Saturday" } };
        
        ObservableCollection<string> specialities = new ObservableCollection<string>(); //список специальностей
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

        bool enabledAddVisit;
        public bool EnabledAddVisit
        {
            get => enabledAddVisit;
            private set => this.RaiseAndSetIfChanged(ref enabledAddVisit, value);
        }

        public ObservableCollection<Patient> FindPatientForAddVisit { get; } = new(); //для поиска пациента

        private string? _searchPatientForAddVisit;
        public string? SearchPatientForAddVisit
        {
            get => _searchPatientForAddVisit;
            set => this.RaiseAndSetIfChanged(ref _searchPatientForAddVisit, value);
        }

        private async void DoSearchPatient(string s)
        {
            VisibleLoad = 100;
            FindPatientForAddVisit.Clear();
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            foreach (var patient in patients)
            {
                string name = $"{patient.Surname} {patient.Name} {patient.Patronymic}";
                if (s != null && ((name.ToLower().Contains(s.ToLower())) || patient.Id.ToString() == s))
                {
                    FindPatientForAddVisit.Add(new Patient(patient.Id, patient.Surname, patient.Name, patient.Patronymic, patient.Phone, patient.DateOfBirth, patient.AreaNumber, patient.Adress));
                }
            }
            if (FindPatientForAddVisit.Count == 1)
            {
                Specialities.Clear();
                EnabledAddVisit = true;
                foreach (var patient in FindPatientForAddVisit)
                {
                    var listSpecialities = doctors.Where(x => x.AreaNumber == patients.First(y => y.Id == patient.Id).AreaNumber).Select(x => x.Speciality);
                    foreach (var item in listSpecialities)
                    {
                        Specialities.Add(item);
                    }
                }
            }
            else EnabledAddVisit = false;
            VisibleLoad = 0;
        }

        public void Appointment()
        {
            CheckData();
        }

        public async void CheckData()
        {
            VisibleLoad = 100;
            ChooseVisible = false; VisibleCard = true;
            int id = FindPatientForAddVisit.First().Id;
            var doctors = await AsyncGetAll<Doctor>("https://localhost:7242/api/Doctor/GetAll");
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var timetables = await AsyncGetAll<Timetable>("https://localhost:7242/api/Timetable/GetAll");
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var doctor = doctors.First(x => x.AreaNumber == patients.First(y => y.Id == id).AreaNumber && x.Speciality == SelectedSpeciality);
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
                    var patientVisit = new Visit(visitNumber, doctor.Id, id, date);
                    var patientComplaint = new Complaints(visitNumber, null, null);
                    await AsyncAdd("https://localhost:7242/api/Visit/Add", patientVisit);
                    await AsyncAdd("https://localhost:7242/api/Complaints/Add", patientComplaint);
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
                            if (!visits.Where(x => x.DoctorId == doctor.Id).Any(x => x.VisitDate == date))
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
                if (Convert.ToUInt32(temp[0].Substring(0, 2)) <= hours && Convert.ToUInt32(temp[1].Substring(0, 2)) > hours) result = true;
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
            int id = FindPatientForAddVisit.First().Id;      
            var date = Convert.ToDateTime(VisitDate);
            date = date.Date + new TimeSpan(Hours, Minutes, 0);
            int visitNumber = visits.OrderBy(x => x.Id).Last().Id + 1;
            var patientVisit = new Visit(visitNumber, doctors.First(x => x.AreaNumber == patients.First(y => y.Id == id).AreaNumber && x.Speciality == SelectedSpeciality).Id, id, date);
            var patientComplaint = new Complaints(visitNumber, null, null);
            await AsyncAdd("https://localhost:7242/api/Visit/Add", patientVisit);
            await AsyncAdd("https://localhost:7242/api/Complaints/Add", patientComplaint);
            TextAppointment = "Пациент записан на приём"; ChooseVisible = false; OnButton = true;
            VisibleLoad = 0;
        }

        public void ChangeData()
        {
            OnButton = true; ChooseVisible = false; TextAppointmentVisible = false;
        }

        public void Switch() //переключение часов
        {
            EnabledHours = !EnabledHours; VisibleHours = !VisibleHours;       
        }

        //Печать справок
        private string? searchTextHelp;
        public string? SearchTextHelp
        {
            get => searchTextHelp;
            set => this.RaiseAndSetIfChanged(ref searchTextHelp, value);
        }

        bool showHelpButtons;
        public bool ShowHelpButtons
        {
            get => showHelpButtons;
            private set => this.RaiseAndSetIfChanged(ref showHelpButtons, value);
        }

        bool visibleListVisits;
        public bool VisibleListVisits
        {
            get => visibleListVisits;
            private set => this.RaiseAndSetIfChanged(ref visibleListVisits, value);
        }

        private string? selectedVisit;
        public string? SelectedVisit
        {
            get => selectedVisit;
            set => this.RaiseAndSetIfChanged(ref selectedVisit, value);
        }

        ObservableCollection<string> listVisitDate = new ObservableCollection<string>(); //список приёмов
        public ObservableCollection<string> ListVisitDate
        {
            get => listVisitDate;
            set => this.RaiseAndSetIfChanged(ref listVisitDate, value);
        }

        public ObservableCollection<Patient> SearchResultsHelp { get; } = new(); //для поиска пациента

        private async void DoSearchPatientHelp(string s)
        {
            VisibleLoad = 100;
            SearchResultsHelp.Clear();
            ListVisitDate.Clear();
            var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            foreach (var patient in patients)
            {
                string name = $"{patient.Surname} {patient.Name} {patient.Patronymic}";
                if (s != null && ((name.ToLower().Contains(s.ToLower())) || patient.Id.ToString() == s))
                {
                    SearchResultsHelp.Add(new Patient(patient.Id, patient.Surname, patient.Name, patient.Patronymic, patient.Phone, patient.DateOfBirth, patient.AreaNumber, patient.Adress));
                }
            }
            if (SearchResultsHelp.Count == 1)
            {
                ShowHelpButtons = true; VisibleListVisits = true;
                var listVisits = visits.Where(x => x.Id == SearchResultsHelp.First().Id);
                foreach (var visit in listVisits) ListVisitDate.Add(visit.VisitDate.ToString("dd/MM/yyyy HH:mm"));
            }
            else { ShowHelpButtons = false; VisibleListVisits = false; }
            VisibleLoad = 0;
        }

        public void PrintVisitDoctor()
        {
        }

        public void PrintSickLeave()
        {
        }

        public void PrintListVisits()
        {
        }

        //Расписание врачей
        public ObservableCollection<TimetableDoctors> SearchResults { get; } = new();

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
            foreach (var doctor in doctors)
            {
                string name = $"{doctor.Surname} {doctor.Name[0]}.{doctor.Patronymic[0]}.";
                var timetable = timetables.FirstOrDefault(x => x.Id == doctor.Id);
                if (timetable != null && s != null && (timetable.OfficeNumber.ToString().Contains(s) || name.ToLower().Contains(s.ToLower())))
                {
                    SearchResults.Add(new TimetableDoctors() { Name = name, Speciality = doctor.Speciality, Office = (int)timetable.OfficeNumber, Days = timetable.VisitDays, Time = timetable.VisitHours });     
                }
            }
            VisibleLoad = 0;
        }

        public void PrintTimetable()
        {
        }

        public RegistrarViewModel(MainViewModel mv)
        {
            MV = mv; LabelDateOfBirth = "Дата рождения"; LabelAdress = "Улица";
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    Streets.Add(ListOfArea[i][j]);
                }
            }

            SearchPatientForAddVisit = ""; LabelSpeciality = "Специальность врача"; OnButton = true; LabelVisitDate = "Дата приёма"; EnabledHours = true; VisibleHours = true; Hours = 0; Minutes = 0; TextTime = "Время приёма";
            this.WhenAnyValue(x => x.SearchPatientForAddVisit).Subscribe(DoSearchPatient!);

            this.WhenAnyValue(x => x.SearchTextHelp).Subscribe(DoSearchPatientHelp!);

            this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch!);

            SearchText = "";
        }

        public void Logout()
        {
            MV.Login();
        }
    }
}
