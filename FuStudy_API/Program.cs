﻿using Microsoft.EntityFrameworkCore;
using FuStudy_Repository.Entity;
using FuStudy_Repository.Repository;
using FuStudy_Model.Mapper;
using AutoMapper;
using FuStudy_Service.Interface;
using FuStudy_Service.Service;
using FuStudy_Service.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FuStudy_Repository;
using FuStudy_Service;
using Net.payOS;
using Quartz.Impl;
using Quartz;
using Tools;
using Tools.Quartz;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"],
    configuration["Environment:PAYOS_API_KEY"],
    configuration["Environment:PAYOS_CHECKSUM_KEY"]);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();


// Thêm đoạn mã sau để đăng ký TimeSpanConverter
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Optional: Make property names case insensitive
    });

/*builder.Services.AddTransient<UpdateConversationIsCloseJob>();

// Cấu hình lịch trình
builder.Services.AddSingleton(provider =>
{
    var schedulerFactory = new StdSchedulerFactory();
    var scheduler = schedulerFactory.GetScheduler().Result;

    // Đăng ký công việc lập lịch
    var updateConversationJob = new JobDetailImpl("UpdateConversationIsCloseJob", typeof(UpdateConversationIsCloseJob));
    var trigger = TriggerBuilder.Create()
        .WithIdentity("UpdateConversationIsCloseTrigger")
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(1)
            .RepeatForever())
        .Build();

    scheduler.ScheduleJob(updateConversationJob, trigger).Wait();
    scheduler.Start().Wait();

    return scheduler;
});*/

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



// Service add o day
//builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRatingService, QuestionRatingService>();
builder.Services.AddScoped<IQuestionCommentService, QuestionCommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubcriptionService, SubcriptionService>();
builder.Services.AddScoped<IStudentSubcriptionService, StudentSubcriptionService>();
builder.Services.AddScoped<IBlogService,BlogService>();
builder.Services.AddScoped<IBlogLikeService, BlogLikeService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IConversationMessageService, ConversationMessageService>();
builder.Services.AddScoped<IMessageReactionService, MessageReactionService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<IMajorService, MajorService>();
builder.Services.AddScoped<IMentorService, MentorService>();
builder.Services.AddScoped<IMentorMajorService, MentorMajorService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IMeetingHistory, MeetingHistoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.Configure<Email>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddScoped<IEmailConfig, EmailConfig>();

builder.Services.AddScoped<Tools.Firebase>();





builder.Services.AddScoped<IBlogCommentService, BlogCommentService>();
builder.Services.AddSingleton(payOS);

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

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});



//Build CORS
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("corspolicy", build =>
    {
        build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FuStudy_API");
        c.RoutePrefix = "";
        c.EnableTryItOutByDefault();
    });
}
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
