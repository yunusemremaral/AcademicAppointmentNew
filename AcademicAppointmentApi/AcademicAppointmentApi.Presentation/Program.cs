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
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext + Identity
builder.Services.AddDbContext<Context>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

// 2) Authentication & JWT: Default scheme olarak JWT'yi ayarla
var jwt = builder.Configuration.GetSection("Jwt");
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

          ValidIssuer = jwt["Issuer"],
          ValidAudience = jwt["Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(jwt["Key"])),
      };
  });

// 3) Dependency Injection
// Generic Repository (Tüm entity'ler için geçerli)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Özel Repositories
builder.Services.AddScoped<ICourseRepository, EfCourseRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
builder.Services.AddScoped<IMessageRepository, EfMessageRepository>();
builder.Services.AddScoped<INotificationRepository, EfNotificationRepository>();
builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();
builder.Services.AddScoped<ISchoolRepository, EfSchoolRepository>();

// Generic Service (Tüm entity'ler için geçerli)
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

// Özel Services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>(); // AppointmentService eklenmeli

// Diðer Servisler
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4) Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
