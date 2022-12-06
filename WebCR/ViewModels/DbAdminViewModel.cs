using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class DbAdminViewModel : ViewModelBase
    {
        MainViewModel mv;
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        //Изменение данных о пользователях

        ObservableCollection<DataLogin> dataLogins = new();
        public ObservableCollection<DataLogin> DataLogins
        {
            get => dataLogins;
            set => this.RaiseAndSetIfChanged(ref dataLogins, value);
        }

        ObservableCollection<string> roles = new() { "Админ данных", "Главврач", "Регистратор", "Врач", "Пациент" };
        public ObservableCollection<string> Roles
        {
            get => roles;
            set => this.RaiseAndSetIfChanged(ref roles, value);
        }

        string? selectedRole;
        public string? SelectedRole
        {
            get => selectedRole;
            set => this.RaiseAndSetIfChanged(ref selectedRole, value);
        }

        int? id;
        public int? Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        string? login;
        public string? Login
        {
            get => login;
            set => this.RaiseAndSetIfChanged(ref login, value);
        }

        string? password;
        public string? Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        int? idPatientOrDoctor;
        public int? IdPatientOrDoctor
        {
            get => idPatientOrDoctor;
            set => this.RaiseAndSetIfChanged(ref idPatientOrDoctor, value);
        }

        public async void UpdateUsers()
        {
            MV.VisibleLoad = 100;
            foreach (var dataLogin in DataLogins)
            {
                await AsyncUpdate($"https://localhost:7242/api/Login/Update/{dataLogin.Id}", dataLogin);
            }
            MV.VisibleLoad = 0;
        }

        public async void DeleteUser()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<DataLogin>($"https://localhost:7242/api/Login/Delete/{Id}");
            DataLogins.Remove(DataLogins.First(x => x.Id == Id));
            MV.VisibleLoad = 0;
        }

        public async void AddUser()
        {
            MV.VisibleLoad = 100;
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");
            await AsyncAdd("https://localhost:7242/api/Login/Add", (new DataLogin(dataLogins.Last().Id + 1, Login, Password, SelectedRole, IdPatientOrDoctor)));
            DataLogins.Add(new DataLogin(dataLogins.Last().Id + 1, Login, Password, SelectedRole, IdPatientOrDoctor));
            MV.VisibleLoad = 0;
        }

        //Препараты

        ObservableCollection<Preparation> preparations = new();
        public ObservableCollection<Preparation> Preparations
        {
            get => preparations;
            set => this.RaiseAndSetIfChanged(ref preparations, value);
        }

        int? preparationNumber;
        public int? PreparationNumber
        {
            get => preparationNumber;
            set => this.RaiseAndSetIfChanged(ref preparationNumber, value);
        }

        string? preparationName;
        public string? PreparationName
        {
            get => preparationName;
            set => this.RaiseAndSetIfChanged(ref preparationName, value);
        }

        public async void UpdatePreparations()
        {
            MV.VisibleLoad = 100;
            foreach (var preparation in Preparations)
            {
                await AsyncUpdate($"https://localhost:7242/api/Preparations/Update/{preparation.Id}", preparation);
            }
            MV.VisibleLoad = 0;
        }

        public async void DeletePreparation()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Timetable>($"https://localhost:7242/api/Preparation/Delete/{PreparationNumber}");
            Preparations.Remove(Preparations.First(x => x.Id == PreparationNumber));
            MV.VisibleLoad = 0;
        }

        public async void AddPreparation()
        {
            MV.VisibleLoad = 100;
            var preparations = await AsyncGetAll<Preparation>("https://localhost:7242/api/Preparation/GetAll");
            await AsyncAdd("https://localhost:7242/api/Preparation/Add", (new Preparation(preparations.Last().Id + 1, PreparationName)));
            Preparations.Add(new Preparation(preparations.Last().Id + 1, PreparationName));
            MV.VisibleLoad = 0;
        }

        //Диагнозы

        ObservableCollection<Diagnosis> diagnosises = new(); //список улиц
        public ObservableCollection<Diagnosis> Diagnosises
        {
            get => diagnosises;
            set => this.RaiseAndSetIfChanged(ref diagnosises, value);
        }

        int? diagnosisNumber;
        public int? DiagnosisNumber
        {
            get => diagnosisNumber;
            set => this.RaiseAndSetIfChanged(ref diagnosisNumber, value);
        }

        string? diagnosisName;
        public string? DiagnosisName
        {
            get => diagnosisName;
            set => this.RaiseAndSetIfChanged(ref diagnosisName, value);
        }

        int? preparationCode;
        public int? PreparationCode
        {
            get => preparationCode;
            set => this.RaiseAndSetIfChanged(ref preparationCode, value);
        }

        public async void UpdateDiagnosises()
        {
            MV.VisibleLoad = 100;
            foreach (var diagnosis in Diagnosises)
            {
                await AsyncUpdate($"https://localhost:7242/api/Diagnosis/Update/{diagnosis.Id}", diagnosis);
            }
            MV.VisibleLoad = 0;
        }

        public async void DeleteDiagnosis()
        {
            MV.VisibleLoad = 100;
            await AsyncDelete<Diagnosis>($"https://localhost:7242/api/Diagnosis/Delete/{DiagnosisNumber}");
            Diagnosises.Remove(Diagnosises.First(x => x.Id == DiagnosisNumber));
            MV.VisibleLoad = 0;
        }

        public async void AddDiagnosis()
        {
            MV.VisibleLoad = 100;
            var diagnosises = await AsyncGetAll<Diagnosis>("https://localhost:7242/api/Diagnosis/GetAll");
            await AsyncAdd("https://localhost:7242/api/Diagnosis/Add", (new Diagnosis(diagnosises.Last().Id + 1, DiagnosisName, PreparationCode)));
            Diagnosises.Add(new Diagnosis(diagnosises.Last().Id + 1, DiagnosisName, PreparationCode));
            MV.VisibleLoad = 0;
        }

        public DbAdminViewModel(MainViewModel mv)
        {
            MV = mv; MV.VisibleLoad = 0;
            Initialize();
        }

        async void Initialize()
        {
            MV.VisibleLoad = 100;
            var dataLogins = await AsyncGetAll<DataLogin>("https://localhost:7242/api/Login/GetAll");
            var diagnosises = await AsyncGetAll<Diagnosis>("https://localhost:7242/api/Diagnosis/GetAll");
            var preparations = await AsyncGetAll<Preparation>("https://localhost:7242/api/Preparation/GetAll");
            foreach (var dataLogin in dataLogins) DataLogins.Add(dataLogin);
            foreach (var preparation in preparations) Preparations.Add(preparation);
            foreach (var diagnosis in diagnosises) Diagnosises.Add(diagnosis);
            MV.VisibleLoad = 0;
        }

        public void Logout()
        {
            MV.VisibleLoad = 100;
            MV.Login();
        }
    }
}
