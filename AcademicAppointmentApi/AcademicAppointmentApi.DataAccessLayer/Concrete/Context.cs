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
                base.OnModelCreating(builder); // Identity tabloları için gerekli

                // School - Department (1-to-Many)
                builder.Entity<School>()
                    .HasMany(s => s.Departments)
                    .WithOne(d => d.School)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.Restrict); // Okul silinirse departmanlar silinmesin

                // Department - AppUser (1-to-Many)
                builder.Entity<Department>()
                    .HasMany(d => d.FacultyMembers)
                    .WithOne(u => u.Department)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); // Departman silinirse kullanıcılar silinmesin

                // Department - Course (1-to-Many)
                builder.Entity<Department>()
                    .HasMany(d => d.Courses)
                    .WithOne(c => c.Department)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade); // Departman silinirse dersler de silinsin

                // Course - AppUser (Instructor) (Many-to-1)
                builder.Entity<Course>()
                    .HasOne(c => c.Instructor)
                    .WithMany(u => u.Courses)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.Restrict); // Hoca silinirse dersler silinmesin

                // Room - AppUser (1-to-1)
                builder.Entity<Room>()
                    .HasOne(r => r.AppUser)
                    .WithOne(u => u.Room)
                    .HasForeignKey<Room>(r => r.AppUserId)
                    .OnDelete(DeleteBehavior.SetNull); // Oda silinirse kullanıcının RoomId'si null olsun

                // Notification - AppUser (Many-to-1)
                builder.Entity<Notification>()
                    .HasOne(n => n.User)
                    .WithMany() // Notification koleksiyonu AppUser'da tanımlı değilse
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse bildirimler de silinsin

                // Message - Sender/Receiver (Many-to-1)
                builder.Entity<Message>()
                    .HasOne(m => m.Sender)
                    .WithMany(u => u.MessagesSent)
                    .HasForeignKey(m => m.SenderId)
                    .OnDelete(DeleteBehavior.Restrict); // Gönderici silinirse mesajlar kalır

                builder.Entity<Message>()
                    .HasOne(m => m.Receiver)
                    .WithMany(u => u.MessagesReceived)
                    .HasForeignKey(m => m.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict); // Alıcı silinirse mesajlar kalır

                // Appointment - AcademicUser & StudentUser (Many-to-1)
                builder.Entity<Appointment>()
                    .HasOne(a => a.AcademicUser)
                    .WithMany(u => u.AppointmentsAsAcademic)
                    .HasForeignKey(a => a.AcademicUserId)
                    .OnDelete(DeleteBehavior.Restrict); // Akademisyen silinirse randevular kalır

                builder.Entity<Appointment>()
                    .HasOne(a => a.StudentUser)
                    .WithMany(u => u.AppointmentsAsStudent)
                    .HasForeignKey(a => a.StudentUserId)
                    .OnDelete(DeleteBehavior.Restrict); // Öğrenci silinirse randevular kalır

                // AppUser - School (Many-to-1)
                builder.Entity<AppUser>()
                    .HasOne(u => u.School)
                    .WithMany() // School'da AppUser koleksiyonu yoksa
                    .HasForeignKey(u => u.SchoolId)
                    .OnDelete(DeleteBehavior.Restrict); // Okul silinirse kullanıcılar kalır

                // AppUser - Room (1-to-1)
                builder.Entity<AppUser>()
                    .HasOne(u => u.Room)
                    .WithOne(r => r.AppUser)
                    .HasForeignKey<Room>(r => r.AppUserId);
            }
        }
    }

