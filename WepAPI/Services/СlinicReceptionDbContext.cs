using Microsoft.EntityFrameworkCore;
using WepAPI.Models;

namespace WepAPI.Services
{
    public class СlinicReceptionDbContext : DbContext
    {
        public DbSet<DataLogin> Log_In { get; set; }
        public DbSet<Patient> Пациент { get; set; }
        public DbSet<Area> Участок { get; set; }
        public DbSet<Preparation> Препарат { get; set; }
        public DbSet<Diagnosis> Диагноз { get; set; }
        public DbSet<Complaints> Жалобы { get; set; }
        public DbSet<Visit> Приём { get; set; }
        public DbSet<SickLeave> Больничный_лист { get; set; }
        public DbSet<Timetable> Расписание { get; set; }
        public DbSet<Survey> Обследование { get; set; }
        public DbSet<Doctor> Врач { get; set; }

        public СlinicReceptionDbContext(DbContextOptions<СlinicReceptionDbContext> options) : base(options) { }
    }
}
