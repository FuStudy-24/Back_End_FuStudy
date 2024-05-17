using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository.Interface;
using FuStudy_Repository.Repository;
using FuStudy_Repository;
using FuStudy_Model.Mapper;
using AutoMapper;
using FuStudy_Service.Interface;
using FuStudy_Service.Service;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Service add o day
//builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


//Mapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton<IMapper>(config.CreateMapper());


// Add services to the container.
var serverVersion = new MySqlServerVersion(new Version(8, 0, 23)); // Replace with your actual MySQL server version
builder.Services.AddDbContext<MyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MyDB");
    options.UseMySql(connectionString, serverVersion, options => options.MigrationsAssembly("FuStudy_API"));
}
);




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




//Build CORS
/*builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    // Dòng ở dưới là đường cứng
    //build.WithOrigins("https:localhost:3000", "https:localhost:7022");

    //Dòng dưới là nhận hết
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));*/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors("MyCors");
app.UseAuthorization();

app.MapControllers();

app.Run();