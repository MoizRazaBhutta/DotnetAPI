using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSalary> UserSalary { get; set; }
        public DbSet<UserJobInfo> UserJobInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnectionString"),optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tell about default schema
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            // As our User Entity is refered to Users in the Database we need to specify it seperately
            modelBuilder.Entity<User>().ToTable("Users","TutorialAppSchema").HasKey(t => t.UserId);
            modelBuilder.Entity<UserSalary>().HasKey(t => t.UserId);
            modelBuilder.Entity<UserJobInfo>().HasKey(t => t.UserId);
        }
    }
}