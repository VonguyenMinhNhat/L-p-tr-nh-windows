using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DS_EF.Models
{
    public partial class StudentContextDB : DbContext
    {
        public StudentContextDB()
            : base("name=StudentContextDB")
        {
        }

        public virtual DbSet<faculty> Faculties { get; set; }
        public virtual DbSet<student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<faculty>()
                .Property(e => e.FacultyName)
                .IsUnicode(false);

            modelBuilder.Entity<student>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<student>()
                .Property(e => e.AverageScore)
                .HasPrecision(5, 2);

        }
    }
}
