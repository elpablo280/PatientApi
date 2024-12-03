using Microsoft.EntityFrameworkCore;
using PatientApi.Models;

namespace PatientApi.Data
{
    public class PatientContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public PatientContext(DbContextOptions<PatientContext> options) : base(options) { }
    }
}
