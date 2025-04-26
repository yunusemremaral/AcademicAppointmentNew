using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;

namespace AcademicAppointmentApi.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, string>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        // DbSets
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Appointment - AcademicUser
            builder.Entity<Appointment>()
                   .HasOne(a => a.AcademicUser)
                   .WithMany(u => u.AppointmentsAsAcademic)
                   .HasForeignKey(a => a.AcademicUserId)
                   .OnDelete(DeleteBehavior.Restrict);  // Randevu silindiğinde kullanıcıyı etkileme

            // Appointment - StudentUser
            builder.Entity<Appointment>()
                   .HasOne(a => a.StudentUser)
                   .WithMany(u => u.AppointmentsAsStudent)
                   .HasForeignKey(a => a.StudentUserId)
                   .OnDelete(DeleteBehavior.Restrict);  // Aynı şekilde

            // Department - School
            builder.Entity<Department>()
                   .HasOne(d => d.School)
                   .WithMany(s => s.Departments)
                   .HasForeignKey(d => d.SchoolId)
                   .OnDelete(DeleteBehavior.Cascade);  // Okul silindiğinde bölüm de silinsin

            // Course - Department
            builder.Entity<Course>()
                   .HasOne(c => c.Department)
                   .WithMany(d => d.Courses)
                   .HasForeignKey(c => c.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);  // Bölüm silindiğinde kurslar silinmesin

            // Course - Instructor (AppUser)
            builder.Entity<Course>()
                   .HasOne(c => c.Instructor)
                   .WithMany()
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);  // Öğretim üyesi silinirse kurs silinmesin

            // Room - AppUser (Her odanın bir kullanıcısı olacak)
            builder.Entity<Room>()
                   .HasOne(r => r.AppUser)
                   .WithOne(u => u.Room)
                   .HasForeignKey<Room>(r => r.AppUserId)
                   .OnDelete(DeleteBehavior.SetNull);  // Oda silindiğinde kullanıcının oda ID'si null olabilir

            // Message - Sender (AppUser)
            builder.Entity<Message>()
                   .HasOne(m => m.Sender)
                   .WithMany(u => u.MessagesSent)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);  // Gönderen kullanıcı silinmesin

            // Message - Receiver (AppUser)
            builder.Entity<Message>()
                   .HasOne(m => m.Receiver)
                   .WithMany(u => u.MessagesReceived)
                   .HasForeignKey(m => m.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);  // Alıcı kullanıcı silinmesin

            // Notification - AppUser
            builder.Entity<Notification>()
                   .HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.SetNull);  // Kullanıcı silindiğinde bildirim null yapılabilir

            // AppUser - Department
            builder.Entity<AppUser>()
                   .HasOne(u => u.Department)
                   .WithMany()
                   .HasForeignKey(u => u.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silindiğinde departman silinmesin

            // AppUser - School
            builder.Entity<AppUser>()
                   .HasOne(u => u.School)
                   .WithMany()
                   .HasForeignKey(u => u.SchoolId)
                   .OnDelete(DeleteBehavior.Restrict);  // Kullanıcı silindiğinde okul silinmesin
        }
    }
}
