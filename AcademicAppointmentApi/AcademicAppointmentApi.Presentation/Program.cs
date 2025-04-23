using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lant�s�
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity yap�land�rmas�
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAppointmentRepository, EfAppointmentRepository> ();

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

app.UseHttpsRedirection();

// Authentication ve Authorization
app.UseAuthentication(); // <-- Bunu da eklemeyi unutma!
app.UseAuthorization();

app.MapControllers();
app.Run();
