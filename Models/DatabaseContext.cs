using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .ToTable("Projects")
                .Property(e => e.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Todo>()
                 .ToTable("Todos")
          .Property(e => e.Created)
          .HasDefaultValueSql("getdate()");
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Todo> Todos => Set<Todo>();


    }
}
