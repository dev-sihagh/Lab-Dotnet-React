using Microsoft.EntityFrameworkCore;
using PatientAdmissionService.Models;

namespace PatientAdmissionService.Data;

public class AdmissionDbContext : DbContext
{
    public AdmissionDbContext(DbContextOptions<AdmissionDbContext> options)
        : base(options) { }

    public DbSet<Patient> Patients { get; set; }

}
