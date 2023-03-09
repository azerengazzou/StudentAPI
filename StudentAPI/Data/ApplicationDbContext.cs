using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 12345,
                    Name = "Name",
                    Email = "emailstudent@gmail.com",
                    Niveau = "5eme",
                    Matricule = "11234567",
                    Created_date = DateTime.Now
                });
        }
    }
}
