using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademicAppointmentApi.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, string>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.AcademicUser)
                      .WithMany(u => u.AppointmentsAsAcademic)
                      .HasForeignKey(a => a.AcademicUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.StudentUser)
                      .WithMany(u => u.AppointmentsAsStudent)
                      .HasForeignKey(a => a.StudentUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.Subject)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(a => a.Description)
                      .HasMaxLength(1000);

                entity.Property(a => a.ScheduledAt)
                      .IsRequired();
            });
        }
    }
}
