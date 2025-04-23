using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.BusinessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabaný baðlantýsý
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Identity yapýlandýrmasý
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAppointmentRepository, EfAppointmentRepository> ();
builder.Services.AddTransient<IEmailService, EmailService>();


// Controller ve Swagger servisi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

// Authentication ve Authorization
app.UseAuthentication(); // <-- Bunu da eklemeyi unutma!
app.UseAuthorization();

app.MapControllers();
app.Run();
