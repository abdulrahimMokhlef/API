using API_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Project.Context
{
  public class InstitutionContext : DbContext
  {
     public InstitutionContext(DbContextOptions<InstitutionContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure unique constraint on DeptName
      modelBuilder.Entity<Department>()
          .HasIndex(d => d.DeptName)
          .IsUnique();

      // One-to-Many Relationship between Department and Student
      modelBuilder.Entity<Student>()
          .HasOne(s => s.Department)
          .WithMany(d => d.Students)
          .HasForeignKey(s => s.DeptId)
          .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
