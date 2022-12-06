using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebCR.Models;

namespace WebCR.ViewModels
{
    public class DoctorViewModel: ViewModelBase
    {
        int? ID { get; set; } //табельный номер врача

        MainViewModel mv;
        public MainViewModel MV
        {
            get => mv;
            private set => this.RaiseAndSetIfChanged(ref mv, value);
        }

        string? selectedVisitDate;
        public string? SelectedVisitDate
        {
            get => selectedVisitDate;
            set => this.RaiseAndSetIfChanged(ref selectedVisitDate, value);
        }

        ObservableCollection<string> listVisitDate = new(); //список приёмов
        public ObservableCollection<string> ListVisitDate
        {
            get => listVisitDate;
            set => this.RaiseAndSetIfChanged(ref listVisitDate, value);
        }

        private bool showPatient;
        public bool ShowPatient
        {
            get => showPatient;
            set => this.RaiseAndSetIfChanged(ref showPatient, value);
        }

        ObservableCollection<string> diagnosises = new();
        public ObservableCollection<string> Diagnosises
        {
            get => diagnosises;
            set => this.RaiseAndSetIfChanged(ref diagnosises, value);
        }

        string? selectedDiagnosis;
        public string? SelectedDiagnosis
        {
            get => selectedDiagnosis;
            set => this.RaiseAndSetIfChanged(ref selectedDiagnosis, value);
        }

        ObservableCollection<string> preparations = new();
        public ObservableCollection<string> Preparations
        {
            get => preparations;
            set => this.RaiseAndSetIfChanged(ref preparations, value);
        }

        string? selectedPreparation;
        public string? SelectedPreparation
        {
            get => selectedPreparation;
            set => this.RaiseAndSetIfChanged(ref selectedPreparation, value);
        }

        string? namePatient;
        public string? NamePatient
        {
            get => namePatient;
            set => this.RaiseAndSetIfChanged(ref namePatient, value);
        }

        string? survey;
        public string? Survey
        {
            get => survey;
            set => this.RaiseAndSetIfChanged(ref survey, value);
        }

        string? sickLeave;
        public string? SickLeave
        {
            get => sickLeave;
            set => this.RaiseAndSetIfChanged(ref sickLeave, value);
        }

        string? complaints;
        public string? Complaints
        {
            get => complaints;
            set => this.RaiseAndSetIfChanged(ref complaints, value);
        }

        public async void FillInfo(string date)
        {
            MV.VisibleLoad = 100;
            if (date != null)
            {
                var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
                var patients = await AsyncGetAll<Patient>("https://localhost:7242/api/Patient/GetAll");
                var surveys = await AsyncGetAll<Survey>("https://localhost:7242/api/Survey/GetAll");
                var sickLeaves = await AsyncGetAll<SickLeave>("https://localhost:7242/api/SickLeave/GetAll");
                var complaints = await AsyncGetAll<Complaints>("https://localhost:7242/api/Complaints/GetAll");
                var diagnosises = await AsyncGetAll<Diagnosis>("https://localhost:7242/api/Diagnosis/GetAll");
                var preparations = await AsyncGetAll<Preparation>("https://localhost:7242/api/Preparation/GetAll");
                var dateTime = DateTime.Parse(date);
                var visit = visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID);
                var patient = patients.First(x => x.Id == visit.CardNumber);
                NamePatient = $"{patient.Surname} {patient.Name[0]}.{patient.Patronymic[0]}.";
                var survey = "не назначено";
                SurveyButtonText = "Назначить обследование";
                if (surveys.Any(x => x.VisitNumber == visit.Id))
                {
                    survey = "";
                    var patientSurveys = surveys.Where(x => x.VisitNumber == visit.Id).OrderBy(y => y.SurveyName);
                    foreach (var s in patientSurveys)
                    {
                        survey += s.SurveyName + ",";
                    }
                    survey = survey[..^1];
                }
                Survey = survey;
                var sickLeave = "нет";
                VisibleSickLeaveButton = true;
                SickLeaveButtonText = "Создать больничный лист";
                if (sickLeaves.Any(x => x.Id == visit.Id && x.Status == "открыт"))
                {
                    sickLeave = "открыт";
                    VisibleSickLeaveButton = false;
                }
                else if (sickLeaves.Any(x => x.Id == visit.Id && x.Status == "закрыт"))
                {
                    sickLeave = "закрыт";
                    VisibleSickLeaveButton = false;
                }
                SickLeave = sickLeave;
                Complaints = complaints.First(x => x.Id == visit.Id).ComplaintsList;
                diagnosises = diagnosises.OrderBy(y => y.DiagnosisName);
                Diagnosis? d = null;
                foreach (var diagnosis in diagnosises)
                {
                    if (complaints.First(x => x.Id== visit.Id).DiagosisCode == diagnosis.Id)
                    {
                        var tb = diagnosis.DiagnosisName;
                        Diagnosises.Add(tb);
                        SelectedDiagnosis = tb;
                        d = diagnosis;
                    }
                    else Diagnosises.Add(diagnosis.DiagnosisName);
                }
                preparations = preparations.OrderBy(y => y.PreapationName);
                foreach (var preparation in preparations)
                {
                    if (d != null && d.PreparationCode == preparation.Id)
                    {
                        var tb = preparation.PreapationName;
                        Preparations.Add(tb);
                        SelectedPreparation = tb;
                    }
                    else Preparations.Add(preparation.PreapationName);
                }
                ShowPatient = true;
            }
            MV.VisibleLoad = 0;
        }

        public async void SavePatientData()
        {
            MV.VisibleLoad = 100;
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var complaints = await AsyncGetAll<Complaints>("https://localhost:7242/api/Complaints/GetAll");
            var diagnosises = await AsyncGetAll<Diagnosis>("https://localhost:7242/api/Diagnosis/GetAll");
            var preparations = await AsyncGetAll<Preparation>("https://localhost:7242/api/Preparation/GetAll");
            var dateTime = DateTime.Parse(SelectedVisitDate);
            var visit = visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID);
            var complaint = complaints.First(x => x.Id == visit.Id);
            if (complaint.ComplaintsList != Complaints)
            {
                complaint.ComplaintsList = Complaints;
                complaint.DiagosisCode = diagnosises.First(x => x.DiagnosisName == SelectedDiagnosis).Id;
                var diagnosis = diagnosises.First(x => x.DiagnosisName == SelectedDiagnosis);
                diagnosis.PreparationCode = preparations.First(x => x.PreapationName == SelectedPreparation).Id;
                await AsyncUpdate($"https://localhost:7242/api/Complaints/Update/{complaint.Id}", complaint);
                await AsyncUpdate($"https://localhost:7242/api/Diagnosis/Update/{diagnosis.Id}", diagnosis);
            }
            MV.VisibleLoad = 0;
        }

        public DoctorViewModel(int? id, MainViewModel mv)
        {
            ID = id; MV = mv; MV.VisibleLoad = 0;
            Initialize();
            this.WhenAnyValue(x => x.SelectedVisitDate).Subscribe(FillInfo!);
        }

        async void Initialize()
        {
            MV.VisibleLoad = 100;
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            var visitDates = visits.Where(x => x.DoctorId == ID).OrderBy(z => z.VisitDate).Select(y => y.VisitDate);
            foreach (var date in visitDates) ListVisitDate.Add(date.ToString("dd/MM/yyyy HH:mm"));
            MV.VisibleLoad = 0;
        }

        public void Logout()
        {
            MV.VisibleLoad = 100;
            MV.Login();
        }

        private bool visibleSickLeaveButton;
        public bool VisibleSickLeaveButton
        {
            get => visibleSickLeaveButton;
            set => this.RaiseAndSetIfChanged(ref visibleSickLeaveButton, value);
        }

        private bool showSurvey;
        public bool ShowSurvey
        {
            get => showSurvey;
            set => this.RaiseAndSetIfChanged(ref showSurvey, value);
        }

        private bool showSickLeave;
        public bool ShowSickLeave
        {
            get => showSickLeave;
            set => this.RaiseAndSetIfChanged(ref showSickLeave, value);
        }

        string? surveyButtonText;
        public string? SurveyButtonText
        {
            get => surveyButtonText;
            set => this.RaiseAndSetIfChanged(ref surveyButtonText, value);
        }

        string? sickLeaveButtonText;
        public string? SickLeaveButtonText
        {
            get => sickLeaveButtonText;
            set => this.RaiseAndSetIfChanged(ref sickLeaveButtonText, value);
        }

        ObservableCollection<string> surveyList = new();
        public ObservableCollection<string> SurveyList
        {
            get => surveyList;
            set => this.RaiseAndSetIfChanged(ref surveyList, value);
        }

        string? curSurvey;
        public string? CurSurvey
        {
            get => curSurvey;
            set => this.RaiseAndSetIfChanged(ref curSurvey, value);
        }

        string? sickLeaveDateOpen;
        public string? SickLeaveDateOpen
        {
            get => sickLeaveDateOpen;
            set => this.RaiseAndSetIfChanged(ref sickLeaveDateOpen, value);
        }

        string? sickLeaveDateClose;
        public string? SickLeaveDateClose
        {
            get => sickLeaveDateClose;
            set => this.RaiseAndSetIfChanged(ref sickLeaveDateClose, value);
        }

        public async void SurveyButton()
        {
            MV.VisibleLoad = 100;
            var surveys = await AsyncGetAll<Survey>("https://localhost:7242/api/Survey/GetAll");
            var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
            if (SurveyButtonText == "Назначить обследование")
            {
                ShowSurvey = true;
                var surveysUniq = surveys.Select(x => x.SurveyName).Distinct();
                foreach (var survey in surveysUniq)
                {
                    SurveyList.Add(survey);
                }
                SurveyButtonText = "Сохранить";
            }
            else
            {
                var dateTime = DateTime.Parse(SelectedVisitDate);
                var patientSurvey = new Survey(surveys.OrderBy(x => x.Id).Last().Id + 1, CurSurvey, visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID).Id);
                await AsyncAdd("https://localhost:7242/api/Survey/Add", patientSurvey);
                ShowSurvey = false;
                var survey = "не назначено";
                SurveyButtonText = "Назначить обследование";
                var visit = visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID);
                if (surveys.Any(x => x.VisitNumber == visit.Id))
                {
                    survey = "";
                    var patientSurveys = surveys.Where(x => x.VisitNumber == visit.Id).OrderBy(y => y.SurveyName);
                    foreach (var s in patientSurveys)
                    {
                        survey += s.SurveyName + ",";
                    }
                    survey = survey[..^1];
                }
                Survey = survey;
            }
            MV.VisibleLoad = 0;
        }

        public async void SickLeaveButton()
        {
            MV.VisibleLoad = 100;
            if (SickLeaveButtonText == "Создать больничный лист")
            {
                ShowSickLeave = true;
                SickLeaveButtonText = "Сохранить";
            }
            else
            {
                SickLeaveButtonText = "Создать больничный лист";
                VisibleSickLeaveButton = false;
                var visits = await AsyncGetAll<Visit>("https://localhost:7242/api/Visit/GetAll");
                var sickLeaves = await AsyncGetAll<SickLeave>("https://localhost:7242/api/SickLeave/GetAll");
                var dateTime = DateTime.Parse(SelectedVisitDate);
                var visit = visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID);
                SickLeave = sickLeaves.First(x => x.Id == visit.Id).Status;
                var patientSickLeave = new SickLeave(visits.First(x => x.VisitDate == dateTime && x.DoctorId == ID).Id, DateTime.Parse(SickLeaveDateOpen), DateTime.Parse(SickLeaveDateClose), "открыт");
                AsyncAdd("https://localhost:7242/api/SickLeave/Add", patientSickLeave);
                ShowSickLeave = false;
            }
            MV.VisibleLoad = 0;
        }
    }
}
