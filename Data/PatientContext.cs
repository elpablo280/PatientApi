using Microsoft.EntityFrameworkCore;
using PatientApi.Models;

namespace PatientApi.Data
{
    public class PatientContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Name> Names { get; set; }

        public PatientContext(DbContextOptions<PatientContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Name>()
                .Property(e => e.Given)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
