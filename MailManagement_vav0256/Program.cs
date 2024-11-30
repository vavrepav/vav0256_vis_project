using MailManagement_vav0256.Services;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories;
using MailManagement_vav0256.Repositories.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MailManagement API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter your custom authorization token in the format **Base64(username:role)**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Register Services
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton<ISenderService, SenderService>();
builder.Services.AddSingleton<IEmailNotificationService, EmailNotificationService>();

// Register Repositories
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<IUserRepository>(_ => new UserRepository(connectionString!));
builder.Services.AddSingleton<IMailRepository>(_ => new MailRepository(connectionString!));
builder.Services.AddSingleton<ISenderRepository>(_ => new SenderRepository(connectionString!));
builder.Services.AddSingleton<IEmailNotificationRepository>(_ => new EmailNotificationRepository(connectionString!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();