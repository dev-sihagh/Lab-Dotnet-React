using Microsoft.EntityFrameworkCore;
using LabApi.Models;

namespace LabApi.Data
{
    public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}
