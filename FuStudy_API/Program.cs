using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Model.Mapper;
using AutoMapper;
using FuStudy_Service.Interface;
using FuStudy_Service.Service;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registering services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Service registrations
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<ISubcriptionService, SubcriptionService>();

// AutoMapper configuration
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton<IMapper>(config.CreateMapper());

// Database context configuration
var serverVersion = new MySqlServerVersion(new Version(8, 0, 23)); // Replace with your actual MySQL server version
builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDB");
    options.UseMySql(connectionString, serverVersion, options => options.MigrationsAssembly("FuStudy_API"));
});

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration (uncomment if needed)
/*builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    // Allow specific origins
    //build.WithOrigins("https://localhost:3000", "https://localhost:7022");

    // Allow all origins
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));*/

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });
    
}

app.UseHttpsRedirection();

// Use CORS (uncomment if needed)
// app.UseCors("MyCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
