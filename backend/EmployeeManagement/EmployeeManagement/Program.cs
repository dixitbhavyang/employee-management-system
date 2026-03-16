using EmployeeManagement.Api.Middleware;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;
using Serilog;

// ========== CONFIGURE SERILOG FIRST (BEFORE BUILDING) ==========
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()  // Log Info and above (Info, Warning, Error)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)  // Reduce ASP.NET noise
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

    // ========================================
    // 1. ADD SERVICES TO CONTAINER
    // ========================================

    // Add Controllers
    builder.Services.AddControllers();

    // Add Infrastructure services (DbContext, Repositories)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Add Application services (Business logic)
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();

    // Add API Documentation (Swagger)
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "Employee Management API",
            Version = "v1",
            Description = "API for managing employees and departments",
            Contact = new()
            {
                Name = "Bhavyang Dixit",
                Email = "your.email@example.com"
            }
        });
    });

    // Add CORS for Angular frontend
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngular", policy =>
        {
            policy.WithOrigins("http://localhost:4200")  // Angular default port
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });

    // ========================================
    // 2. BUILD THE APP
    // ========================================

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // ========================================
    // 3. CONFIGURE HTTP REQUEST PIPELINE
    // ========================================

    // Development-only middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
            options.RoutePrefix = string.Empty;  // Swagger at root (http://localhost:5000)
        });
    }

    // HTTPS Redirection
    app.UseHttpsRedirection();

    // CORS
    app.UseCors("AllowAngular");

    app.UseAuthorization();

    // Map Controllers
    app.MapControllers();

    Log.Information("Employee Management API started successfully");

    // ========================================
    // 4. RUN THE APP
    // ========================================

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