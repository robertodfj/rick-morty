// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var httpClient = new HttpClient();
var apiSettings = new ApiSettings(); // aquí ya está "http://localhost:5235/"
var charactersService = new CharactersService(httpClient, apiSettings.URL);

var characters = await charactersService.GetCharactersAsync();