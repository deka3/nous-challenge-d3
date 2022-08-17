using CleaningManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CleaningManagement.DAL
{
    public class CleaningManagementDbContext : DbContext
    {
        public string DbPath { get; }

        public CleaningManagementDbContext()
        {
        }

        public CleaningManagementDbContext(DbContextOptions<CleaningManagementDbContext> options)
            : base(options)
        {
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("CleaningContext");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CleaningPlan>()
                        .Property(b => b.CreatedAt)
                        .HasDefaultValue(DateTime.UtcNow);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CleaningPlan> CleaningPlans { get; set; }

        public DbSet<User> Users { get; set; }
    }
}