using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Bot.model.request;

namespace Bot.service
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public AuthService(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<(bool Success, string Message)> Register(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/register", registerRequest);
            var content = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, content);
        }

        public async Task<(bool Success, string Message)> RegisterAdmin(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/register-admin", registerRequest);
            var content = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, content);
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/login", loginRequest);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}