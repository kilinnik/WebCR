using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class RegistrationViewModel: ViewModelBase
    {
        //список улиц по участкам
        static readonly List<List<string>> ListOfArea = new() { new List<string> {"ул. им. 40-летия Победы", "ул. Островского", "Коллективная ул." }, new List<string> {"ул. Жлобы", "ул. МОПР", "Первомайская ул.", "ул. 1 Мая" }, new List<string> {"ул. Филатова", "Школьная ул." } };
       
        int? indexCurStreet; //индекс текущей улицы
        public int? IndexCurStreet 
        {
            get => indexCurStreet;
            set => this.RaiseAndSetIfChanged(ref indexCurStreet, value);
        }

        ObservableCollection<string> streets = new(); //список улиц
        public ObservableCollection<string> Streets
        {
            get => streets;
            set => this.RaiseAndSetIfChanged(ref streets, value);
        }

        string? selectedStreet; //выбраная улица
        public string? SelectedStreet
        {
            get => selectedStreet;
            set => this.RaiseAndSetIfChanged(ref selectedStreet, value);
        }

        int? house; //№ дома
        public int? House
        {
            get => house;
            set => this.RaiseAndSetIfChanged(ref house, value);
        }

        int? flat; //№ квартиры
        public int? Flat
        {
            get => flat;
            set => this.RaiseAndSetIfChanged(ref flat, value);
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

        string? labelLogin;
        public string? LabelLogin
        {
            get => labelLogin; 
            set => this.RaiseAndSetIfChanged(ref labelLogin, value);
        }

        string? labelPassword;
        public string? LabelPassword
        {
            get => labelPassword;
            set => this.RaiseAndSetIfChanged(ref labelPassword, value);
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

        string? dateOfBirth;
        public string? DateOfBirth
        {
            get => dateOfBirth;
            private set => this.RaiseAndSetIfChanged(ref dateOfBirth, value);
        }

        string? login;
        public string? Login
        {
            get => login;
            private set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string? password;
        public string? Password
        {
            get => password;
            private set => this.RaiseAndSetIfChanged(ref password, value);
        }

        MainViewModel mv;
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        bool showFloatingWatermark;
        public bool ShowFloatingWatermark
        {
            get => showFloatingWatermark;
            private set => this.RaiseAndSetIfChanged(ref showFloatingWatermark, value);
        }

        private static readonly MaterialTheme MaterialThemeStyles = Application.Current!.LocateMaterialTheme<MaterialTheme>();

        bool isDark;

        public void ChangeTheme()
        {
            MaterialThemeStyles.BaseTheme = isDark ? BaseThemeMode.Light : BaseThemeMode.Dark;
            isDark = !isDark;
        }

        public async void Registration()
        {
            MV.VisibleLoad = 100;
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");
            LabelSurname = null; LabelName = null; LabelPatronymic = null; LabelPhone = null; LabelDateOfBirth = null; LabelAdress = null; LabelLogin = null; LabelPassword = null;
            if (Surname == null) LabelSurname = "Введите фамилию";
            else if (!Surname.All(Char.IsLetter)) LabelSurname = "Фамилия содержит недопустимые символы";
            if (Name == null) LabelName = "Введите имя";
            else if (!Name.All(Char.IsLetter)) LabelName = "Имя содержит недопустимые символы";
            if (Patronymic == null) LabelPatronymic = "Введите отчество";
            else if (!Patronymic.All(Char.IsLetter)) LabelPatronymic = "Отчество содержит недопустимые символы";
            if (Phone == null) LabelPhone = "Введите номер телефона";
            else if (Phone.ToString()?.Length != 11) LabelPhone = "Неверный формат номера телефона";
            if (DateOfBirth == null) LabelDateOfBirth = "Выберете дату рождения";
            else if (Convert.ToDateTime(DateOfBirth) > DateTime.Today) LabelDateOfBirth = "Некорректная дата рождения";
            if (SelectedStreet == null || House == null) LabelAdress = "Введите адрес";
            if (Login == null) LabelLogin = "Введите логин";
            else if (dataLogins.Any(x => x.Login == Login)) LabelLogin = "Такой логин уже существует";
            if (Password == null) LabelPassword = "Введите пароль";
            if (LabelSurname == null && LabelName == null && LabelPatronymic == null && LabelPhone == null && LabelDateOfBirth == null && LabelAdress == null && LabelLogin == null && LabelPassword == null)
            {
                int area = IndexCurStreet < ListOfArea[0].Count ? 1 : IndexCurStreet < (ListOfArea[0].Count + ListOfArea[1].Count) ? 2 : 3;
                string adress;
                if (Flat == null) adress = $"{SelectedStreet}, д. {House}";
                else adress = $"{SelectedStreet}, д. {House}, кв. {Flat}";
                var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
                Patient patient = new(patients.Last().Id + 1, Surname, Name, Patronymic, Phone, Convert.ToDateTime(DateOfBirth), area, adress);
                await AsyncAdd("https://localhost:7242/api/Patient/Add", patient);
                DataLogin login = new(dataLogins.Last().Id + 1, Login, Password, "Пациент", patient.Id);
                await AsyncAdd("https://localhost:7242/api/Login/Add", login);
                MV.Patient(patient.Id);
            }
            else ShowFloatingWatermark = true;
            MV.VisibleLoad = 0;
        }

        public RegistrationViewModel(MainViewModel mw)
        {
            isDark = MaterialThemeStyles.BaseTheme == BaseThemeMode.Dark;
            LabelDateOfBirth = "Дата рождения"; MV = mw; LabelAdress = "Улица"; 
            for (int i = 0; i < ListOfArea.Count; i++)
            {
                for (int j = 0; j < ListOfArea[i].Count; j++)
                {
                    Streets.Add(ListOfArea[i][j]);
                }
            }
            MV.VisibleLoad = 0;
        }

        public void Back()
        {
            MV.VisibleLoad = 100;
            MV.Login();
        }
    }
}
