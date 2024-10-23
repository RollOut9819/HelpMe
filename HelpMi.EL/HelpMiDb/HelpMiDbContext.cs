using HelpMi.EL.HelpMiDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using HelpMi.Infrastructure;

namespace HelpMi.EL.HelpMiDb
{
    public class HelpMiDbContext : DbContext
    {
        public HelpMiDbContext(DbContextOptions<HelpMiDbContext> options)
       : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mappare le entità agli schemi specifici
            modelBuilder.Entity<User>().ToTable("User", schema: "user");
            modelBuilder.Entity<Ticket>().ToTable("Ticket", schema: "dbo");
            modelBuilder.Entity<Status>().ToTable("Status", schema: "dbo");
            modelBuilder.Entity<Priority>().ToTable("Priority", schema: "dbo");
            modelBuilder.Entity<Category>().ToTable("Category", schema: "dbo");           

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Infrastructure.ConnectionStrings.OcStanceConnection);
        }
    }
}
