using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademicAppointmentApi.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, string>
    {
        public Context(DbContextOptions<Context> opts) : base(opts) { }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // FK kısıtlamaları
            builder.Entity<Appointment>()
                   .HasOne(a => a.AcademicUser)
                   .WithMany(u => u.AppointmentsAsAcademic)
                   .HasForeignKey(a => a.AcademicUserId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Appointment>()
                   .HasOne(a => a.StudentUser)
                   .WithMany(u => u.AppointmentsAsStudent)
                   .HasForeignKey(a => a.StudentUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
