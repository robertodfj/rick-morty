using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using RickYMorty.middleware;
using RickYMorty.data;
using RickYMorty.service.auth;
using RickYMorty.service;
using RickYMorty.token;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Registrar APP DBContext
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EpisodeService>();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<TradeService>();
builder.Services.AddScoped<UserService>();



builder.Services.AddLogging();
builder.Services.AddHttpClient();

// Añadimos el JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = ValidateToken.GetTokenValidationParameters(builder.Configuration);

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "Authentication token is missing or invalid"
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    };
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
// Probar con postman

// Subir a vercel

// Crear el bot de telegram desde el que poder jugar