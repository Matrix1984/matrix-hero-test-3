using AutoMapper;
using hero_trainer_app.AutoMapperProfiles;
using hero_trainer_app.Middlewares;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Repositories.HeroRepo;
using Infrastructure.Repositories.TrainingSessionRepo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using NLog;
using NLog.Web;
using System.Text;
using System.Text.RegularExpressions;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddControllersWithViews();

    builder.Logging.ClearProviders();

    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Host.UseNLog();

    builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

    builder.Services.AddScoped<IHeroRepository, HeroRepository>();

    builder.Services.AddScoped<ITrainingSessionRepository, TrainingSessionRepository>();

    builder.Services.AddDbContext<HeroDbContext>(opt =>
      opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: postgreOptions =>
        {
            postgreOptions.MigrationsAssembly("Application");
        }));


    builder.Services.AddIdentity<Trainer, IdentityRole>(options =>
    {
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 #$^+=!*()@%&";
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
        options.User.RequireUniqueEmail = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
    })
    .AddEntityFrameworkStores<HeroDbContext>()
    .AddDefaultTokenProviders();



    // Adding Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    // Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

  

    var app = builder.Build();

    app.UseCors(builder => builder
             .SetIsOriginAllowed(
                origin =>
                {
                    return Regex.IsMatch(origin, @"^https?://localhost[^\.]*$");
                })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
     
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        //Centrailized logger.
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
     

    app.MapFallbackToFile("index.html"); ;

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
