using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.BusinessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                          Encoding.UTF8.GetBytes(jwtSection["Key"]))
        };
    });
builder.Services.AddAuthorization();


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

app.UseHttpsRedirection();
app.UseCors();


// Authentication ve Authorization
app.UseAuthentication(); // <-- Bunu da eklemeyi unutma!
app.UseAuthorization();

app.MapControllers();
app.Run();
