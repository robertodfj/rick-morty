var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


// Añadir un limite para trabajar (cada 30min añadir un date time en user model y comprobar en el service si ha pasado el tiempo para poder trabajar de nuevo, y añadir un contador de veces que se ha trabajado para aumentar la probabilidad de captura)
// Añadir un bool de si el character/episode esta en venta o no, y un precio, para poder comprar y vender entre usuarios
// Añadir un service para mostrar los que estan en venta
// Añadir el service para comprar y vender con validaciones de dinero usr etc, añadir instantaneamente el dinero al que quiere poner el personaje(como el fantasy) nada mas comprarlo
// Añadir probabilidad de caza pequeña para el principio no valgan dinero y haya que trabajar para comprar a otros usuarios, (Se puede añadir level y cuanto mas veces trabajes y captures mas facil sea capturar)
// https://rickandmortyapi.com/api/episode/51

// Crear un manejo de errores personalizado para la API, con un middleware

// Crear el token y uso de jwt para autenticación y autorización
// Crear los controladores para los personajes y episodios, con sus respectivos servicios

// Crear el program cs

// Probar con postman

// Subir a vercel

// Crear el bot de telegram desde el que poder jugar