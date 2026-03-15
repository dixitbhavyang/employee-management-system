using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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

// Authorization (we'll add authentication later)
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// ========================================
// 4. RUN THE APP
// ========================================

app.Run();