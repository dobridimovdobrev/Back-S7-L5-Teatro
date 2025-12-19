using Teatro.Data;
using Teatro.Models.Entities;
using Teatro.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Information("Registering services...");

builder.Services.AddSerilog();

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "server.com",
            ValidAudience = "client.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uRcA4z5PNgJH2vLq9Xy7TfK3wB8mS6cZ"))
        }
    );

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configurazione identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.Password.RequiredLength = 8;
    option.Password.RequireDigit = true;
    option.Password.RequireUppercase = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireNonAlphanumeric = true;

})
 .AddRoles<ApplicationRole>()
 .AddEntityFrameworkStores<ApplicationDbContext>()
 .AddSignInManager();

builder.Services.AddAuthorization();

try
{
    string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ConfigurationException("Stringa di connessione non trovata");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection)
);
}
catch (ConfigurationException ex)
{
    Log.Fatal(ex.Message);
    await Log.CloseAndFlushAsync();
    Environment.Exit(1);
}
catch (Exception ex)
{
    Log.Fatal(ex.Message);
    await Log.CloseAndFlushAsync();
    Environment.Exit(1);
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
