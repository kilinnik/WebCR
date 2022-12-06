using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WepAPI.Models
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
    }
    public class Patient : DomainObject
    {
        [Column("Фамилия")]
        public string? Surname { get; set; }
        [Column("Имя")] 
        public string? Name { get; set; }

        [Column("Отчество")] 
        public string? Patronymic { get; set; }
        [Column("Телефон")]
        public long? Phone { get; set; }
        [Column("Дата_рождения")]
        public DateTime DateOfBirth { get; set; }
        [Column("Номер_участка")]
        public int? AreaNumber { get; set; }
        [Column("Адрес")] 
        public string? Adress { get; set; }
    }

    public class Area : DomainObject
    {
        [Column("Адрес_участка")]
        public string? AreaAdress { get; set; }
    }

    public class Preparation : DomainObject
    {
        [Column("Название_препарата")]
        public string? PreapationName { get; set; }
    }

    public class Diagnosis : DomainObject
    {
        [Column("Название")]
        public string? DiagnosisName { get; set; }
        [Column("Код_препарата")]
        public int? PreparationCode { get; set; }
    }

    public class Complaints : DomainObject
    {
        [Column("Жалобы")]
        public string? ComplaintsList { get; set; }
        [Column("Код_диагноза")]
        public int? DiagosisCode { get; set; }
    }

    public class Visit : DomainObject
    {
        [Column("Табельный_номер")]
        public int? DoctorId { get; set; }
        [Column("Номер_карты")]
        public int? CardNumber { get; set; }
        [Column("Дата_приёма")]
        public DateTime VisitDate { get; set; }
    }

    public class SickLeave : DomainObject
    {
        [Column("Открыт")]
        public DateTime? Open { get; set; }
        [Column("Закрыт")]
        public DateTime? Close { get; set; }
        [Column("Статус")]
        public string? Status { get; set; }
    }

    public class Timetable : DomainObject
    {
        [Column("Дни_приёма")]
        public string? VisitDays { get; set; }
        [Column("Часы_приёма")]
        public string? VisitHours { get; set; }
        [Column("Номер_кабинета")]
        public int? OfficeNumber { get; set; }
    }

    public class Survey : DomainObject
    {
        [Column("Название_обследования")]
        public string? SurveyName { get; set; }
        [Column("Номер_визита")]
        public int? VisitNumber { get; set; }
    }

    public class Doctor : DomainObject
    {
        [Column("Фамилия")]
        public string? Surname { get; set; }
        [Column("Имя")]
        public string? Name { get; set; }
        [Column("Отчество")] 
        public string? Patronymic { get; set; }
        [Column("Дата_приёма_на_работу")]
        public DateTime? HireDate { get; set; }
        [Column("Стаж")]
        public int? Experience { get; set; }
        [Column("Адрес")]
        public string? Adress { get; set; }
        [Column("Специальность")]
        public string? Speciality { get; set; }
        [Column("Номер_участка")]
        public int? AreaNumber { get; set; }
        [Column("Телефон")]
        public long? Phone { get; set; }
    }
}
