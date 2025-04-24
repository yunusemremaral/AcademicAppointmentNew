using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;

namespace AcademicAppointmentApi.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, string>
    {
        public Context(DbContextOptions<Context> opts) : base(opts) { }

        // DbSet'ler
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

            // Appointment ilişkisi
            builder.Entity<Appointment>()
                   .HasOne(a => a.AcademicUser)
                   .WithMany(u => u.AppointmentsAsAcademic)
                   .HasForeignKey(a => a.AcademicUserId)
                   .OnDelete(DeleteBehavior.Restrict);  // Silme kısıtlaması uygulandı

            builder.Entity<Appointment>()
                   .HasOne(a => a.StudentUser)
                   .WithMany(u => u.AppointmentsAsStudent)
                   .HasForeignKey(a => a.StudentUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // School ve Department ilişkisi
            builder.Entity<Department>()
                   .HasOne(d => d.School)
                   .WithMany(s => s.Departments)
                   .HasForeignKey(d => d.SchoolId)
                   .OnDelete(DeleteBehavior.Cascade); // Okul silindiğinde bölüm de silinsin.

            // Department ve Course ilişkisi
            builder.Entity<Course>()
                   .HasOne(c => c.Department)
                   .WithMany(d => d.Courses)
                   .HasForeignKey(c => c.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Course ve Instructor ilişkisi
            builder.Entity<Course>()
                   .HasOne(c => c.Instructor)
                   .WithMany()
                   .HasForeignKey(c => c.InstructorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Room ve Department ilişkisi
            builder.Entity<Room>()
                   .HasOne(r => r.Department)
                   .WithMany(d => d.Rooms)
                   .HasForeignKey(r => r.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);  // Silinirse oda null yapılacak

            // Message ve AppUser ilişkisi (sender ve receiver ilişkisi)
            builder.Entity<Message>()
                   .HasOne(m => m.Sender)
                   .WithMany(u => u.Messages)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                   .HasOne(m => m.Receiver)
                   .WithMany()
                   .HasForeignKey(m => m.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Notification ve AppUser ilişkisi
            builder.Entity<Notification>()
                   .HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.SetNull); // Kullanıcı silindiğinde bildirim null yapılabilir

            // AspNetUsers ve Department ilişkisi
            builder.Entity<AppUser>()
                   .HasOne(u => u.Department)
                   .WithMany()
                   .HasForeignKey(u => u.DepartmentId)
                   .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde departman silinmez.

            // AspNetUsers ve School ilişkisi
            builder.Entity<AppUser>()
                   .HasOne(u => u.School)
                   .WithMany()
                   .HasForeignKey(u => u.SchoolId)
                   .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde okul silinmez.

            // Room ve AppUser ilişkisi (AssignedInstructors)
            builder.Entity<Room>()
                   .HasMany(r => r.AssignedInstructors)
                   .WithMany(u => u.AssignedRooms)
                   .UsingEntity(j => j.ToTable("RoomInstructors"));
        }
    }
}
