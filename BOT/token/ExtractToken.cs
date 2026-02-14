/*
    Clase utilizada para convertir
{
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSN"
}

    En un string como: eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSN
 */
using System.Text.Json;

namespace Bot.token
{
    public class ExtractToken
    {
        public string GetTokenFromResponse(string response)
        {
            try
            {
                var doc = JsonDocument.Parse(response);
                if(doc.RootElement.TryGetProperty("token", out var jsonElement))
                {
                    return jsonElement.GetString() ?? string.Empty;
                }
                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}