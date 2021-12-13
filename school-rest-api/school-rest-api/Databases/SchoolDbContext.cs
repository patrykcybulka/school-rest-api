using Microsoft.EntityFrameworkCore;
using school_rest_api.Entries;

namespace school_rest_api.Databases
{
    public class SchoolDbContext : DbContext
    {
        public DbSet<ClassEntry> Classes { get; set; }
        public DbSet<EducatorEntry> Educators { get; set; }
        public DbSet<StudentEntry> Students { get; set; }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassEntry>().ToTable("Classes");
            modelBuilder.Entity<EducatorEntry>().ToTable("Educators");
            modelBuilder.Entity<StudentEntry>().ToTable("Students");
        }
    }
}
