using Avalonia;
using ReactiveUI;
using System.Collections.Generic;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        MainViewModel mv; //для смены view
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        int showMesWrongData; //для сообщения о неправильных данных
        public int ShowMesWrongData
        {
            get => showMesWrongData;
            private set => this.RaiseAndSetIfChanged(ref showMesWrongData, value);
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

        private static readonly MaterialTheme MaterialThemeStyles = Application.Current!.LocateMaterialTheme<MaterialTheme>();

        bool isDark;

        public void ChangeTheme() 
        {
            MaterialThemeStyles.BaseTheme = isDark ? BaseThemeMode.Light : BaseThemeMode.Dark;
            isDark= !isDark;
        }

        private int visibleLoad;
        public int VisibleLoad
        {
            get => visibleLoad;
            set => this.RaiseAndSetIfChanged(ref visibleLoad, value);
        }

        public async void CheckLogin()
        {
            VisibleLoad = 100;
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");
            var login = dataLogins.FirstOrDefault(x => x.Login == Login && x.Password == Password);
            if (login != null)
            {
                if (login.Role == "Админ БД") MV.DbAdmin();
                if (login.Role == "Админ данных") MV.DataAdmin();
                if (login.Role == "Главврач") MV.HeadDoctor();
                if (login.Role == "Регистратор") MV.Registrar();
                if (login.Role == "Врач") MV.Doctor(login.IdPatientOrDoctor);
                if (login.Role == "Пациент") MV.Patient(login.IdPatientOrDoctor);
            }
            else ShowMesWrongData = 100;
            VisibleLoad = 0;
        }

        public LoginViewModel(MainViewModel mw)
        {
            isDark = MaterialThemeStyles.BaseTheme == BaseThemeMode.Dark; MV = mw;
        }

        public void Registration() //view регистрации
        {
            VisibleLoad = 100;
            MV.Registration();
        }
    }
}
