using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using EF_Core_DAL.Entities;

namespace EF_Core_DAL
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(e =>
            {
                e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(29);
                e.HasIndex(p=>p.Name,$"IX_{nameof(Tag)}_{nameof(Tag.Name)}").IsUnique();

                e.Property(p => p.Description)
              .HasMaxLength(400)
                .IsRequired(false);
               

            });
            modelBuilder.Entity<User>(e =>
            {
                e.Property(p => p.MiddleName)
                .IsRequired(false);
                e.HasIndex(p => new {p.Email, p.PhoneNumber }, 
                    $"IX_Unique_{nameof(User.Email)}{nameof(User.PhoneNumber)}").IsUnique();
            });

            modelBuilder.Entity<Task>()
            
                .Property(p => p.Description)
                .IsRequired(false);
               
            base.OnModelCreating(modelBuilder);
        }
    }
}
