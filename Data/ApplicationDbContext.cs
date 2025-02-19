using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaskItem>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.CreatedBy)   
                .WithMany(u => u.Tasks)     
                .HasForeignKey(t => t.CreatedById)  
                .IsRequired()              
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}