using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Teatro.Data;
using Teatro.Exceptions;
using Teatro.Helpers;
using Teatro.Models.Entities;
using Teatro.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Information("Registering services...");

builder.Services.AddSerilog();

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

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Information("Services registered successfully");
Log.Information("Building application...");

//registrare servizi

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ArtistaService>();
builder.Services.AddScoped<EventoService>();
builder.Services.AddScoped<BigliettoService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    try
    {
        await DbHelper.InitializeDatabaseAsync<ApplicationDbContext>(app);
        Log.Information("Database inizializzato con successo");
    }
    catch (DbInitializationException ex)
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
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Log.Information("Running application...");

app.Run();