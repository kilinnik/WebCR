using System;

namespace WebCR.Models
{
    public class DomainObject
    {
        public int Id { get; set; }
    }

    public class DataLogin : DomainObject
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? IdPatientOrDoctor { get; set; }
        public DataLogin(int id, string login, string password, string role, int? idPatientOrDoctor)
        {
            Id = id;
            Login = login;
            Password = password;
            Role = role;
            IdPatientOrDoctor = idPatientOrDoctor;
        }
    }
    public class Patient : DomainObject
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public long? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? AreaNumber { get; set; }
        public string? Adress { get; set; }
        public Patient(int id, string? surname, string? name, string? patronymic, long? phone, DateTime dateOfBirth, int? areaNumber, string? adress)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Phone = phone;
            DateOfBirth = dateOfBirth;
            AreaNumber = areaNumber;
            Adress = adress;
        }
    }

    public class Area : DomainObject
    {
        public string? AreaAdress { get; set; }
        public Area(int id, string? areaAdress)
        {
            Id = id;
            AreaAdress = areaAdress;
        }
    }

    public class Preparation : DomainObject
    {
        public string? PreapationName { get; set; }
        public Preparation(int id, string? preapationName)
        {
            Id = id;
            PreapationName = preapationName;
        }
    }

    public class Diagnosis : DomainObject
    {
        public string? DiagnosisName { get; set; }
        public int? PreparationCode { get; set; }
        public Diagnosis(int id, string? diagnosisName, int? код_препарата)
        {
            Id = id;
            DiagnosisName = diagnosisName;
            PreparationCode = код_препарата;
        }
    }

    public class Complaints : DomainObject
    {
        public string? ComplaintsList { get; set; }
        public int? DiagosisCode { get; set; }
        public Complaints(int id, string? complaintsList, int? diagosisCode)
        {
            Id = id;
            ComplaintsList = complaintsList;
            DiagosisCode = diagosisCode;
        }
    }

    public class Visit : DomainObject
    {
        public int? DoctorId { get; set; }
        public int? CardNumber { get; set; }
        public DateTime VisitDate { get; set; }
        public Visit(int id, int? doctorId, int? cardNumber, DateTime visitDate)
        {
            Id = id;
            DoctorId = doctorId;
            CardNumber = cardNumber;
            VisitDate = visitDate;
        }
    }

    public class SickLeave : DomainObject
    {
        public DateTime? Open { get; set; }
        public DateTime? Close { get; set; }
        public string? Status { get; set; }
        public SickLeave(int id, DateTime? open, DateTime? close, string? status)
        {
            Id = id;
            Open = open;
            Close = close;
            Status = status;
        }
    }

    public class Timetable : DomainObject
    {
        public string? VisitDays { get; set; }
        public string? VisitHours { get; set; }
        public int? OfficeNumber { get; set; }
        public Timetable(int id, string? visitDays, string? visitHours, int? officeNumber)
        {
            Id = id;
            VisitDays = visitDays;
            VisitHours = visitHours;
            OfficeNumber = officeNumber;
        }
    }

    public class Survey : DomainObject
    {
        public string? SurveyName { get; set; }
        public int? VisitNumber { get; set; }
        public Survey(int id, string? surveyName, int? visitNumber)
        {
            Id = id;
            SurveyName = surveyName;
            VisitNumber = visitNumber;
        }
    }

    public class Doctor : DomainObject
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? HireDate { get; set; }
        public int? Experience { get; set; }
        public string? Adress { get; set; }
        public string? Speciality { get; set; }
        public int? AreaNumber { get; set; }
        public long? Phone { get; set; }
        public Doctor(int id, string? surname, string? name, string? patronymic, DateTime? hireDate, int? experience, string? adress, string? speciality, int? areaNumber, long? phone)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            HireDate = hireDate;
            Experience = experience;
            Adress = adress;
            Speciality = speciality;
            AreaNumber = areaNumber;
            Phone = phone;
        }
    }
}
