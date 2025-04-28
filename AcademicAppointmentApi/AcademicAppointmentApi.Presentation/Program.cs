    using AcademicAppointmentApi.BusinessLayer.Abstract;
    using AcademicAppointmentApi.BusinessLayer.Concrete;
    using AcademicAppointmentApi.DataAccessLayer.Abstract;
    using AcademicAppointmentApi.DataAccessLayer.Concrete;
    using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
    using AcademicAppointmentApi.EntityLayer.Entities;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
using System.Text.Json;

    var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 1) DbContext + Identity
builder.Services.AddDbContext<Context>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<AppUser, AppRole>(opts =>
    {
        opts.User.RequireUniqueEmail = true;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

    // 2) JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

    // 3) Dependency Injection (DI)
    // Generic Repositories & Services
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddScoped(typeof(ITGenericService<>), typeof(TGenericService<>));

    // Entity-Specific Repositories
    builder.Services.AddScoped<IAppointmentRepository, EfAppointmentRepository>();
    builder.Services.AddScoped<ICourseRepository, EfCourseRepository>();
    builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
    builder.Services.AddScoped<IMessageRepository, EfMessageRepository>();
    builder.Services.AddScoped<INotificationRepository, EfNotificationRepository>();
    builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();
    builder.Services.AddScoped<ISchoolRepository, EfSchoolRepository>();

    // Entity-Specific Services
    builder.Services.AddScoped<IAppointmentService, AppointmentService>();
    builder.Services.AddScoped<ICourseService, CourseService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IMessageService, MessageService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    builder.Services.AddScoped<IRoomService, RoomService>();
    builder.Services.AddScoped<ISchoolService, SchoolService>();

    // Utility Services
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

    // API Configuration
    builder.Services.AddHttpContextAccessor();
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
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            }));
        }
    });
});


app.UseHttpsRedirection();

    app.UseCors("AllowAll");

app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();