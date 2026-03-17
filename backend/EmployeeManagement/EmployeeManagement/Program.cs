using EmployeeManagement.Api.Middleware;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

// ========== CONFIGURE SERILOG FIRST ==========
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting Employee Management API");

    var builder = WebApplication.CreateBuilder(args);

    // ========== USE SERILOG ==========
    builder.Host.UseSerilog();

    // ========== JWT SETTINGS ==========
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");

    // ========== AUTHENTICATION ==========
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning("Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var username = context.Principal?.Identity?.Name;
                Log.Information("Token validated for user: {Username}", username);
                return Task.CompletedTask;
            }
        };
    });

    builder.Services.AddAuthorization();

    // ========== SERVICES ==========
    builder.Services.AddControllers();

    // Infrastructure services (DbContext, Repositories)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Application services (Business logic)
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IAuthService, AuthService>();

    // API Documentation
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    // CORS - Allow Angular frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // ========== BUILD APP ==========
    var app = builder.Build();

    // ========== MIDDLEWARE PIPELINE (ORDER MATTERS!) ==========

    // 1. Request Logging
    app.UseSerilogRequestLogging();

    // 2. Exception Handling
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 3. API Documentation (Development only)
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Employee Management API")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
    }

    // 4. HTTPS Redirection
    app.UseHttpsRedirection();

    // 5. CORS
    app.UseCors("AllowAngular");

    // 6. Authentication (WHO you are)
    app.UseAuthentication();

    // 7. Authorization (WHAT you can do)
    app.UseAuthorization();

    // 8. Controllers
    app.MapControllers();

    Log.Information("Employee Management API started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}