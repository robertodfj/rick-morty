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
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/auth/register")
            {
                Content = JsonContent.Create(registerRequest)
            };
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, content);
        }

        public async Task<(bool Success, string Message)> RegisterAdmin(RegisterRequest registerRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/auth/register-admin")
            {
                Content = JsonContent.Create(registerRequest)
            };
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, content);
        }

        public async Task<(bool Success, string Message)> Login(LoginRequest loginRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/auth/login")
            {
                Content = JsonContent.Create(loginRequest)
            };
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, content);
        }
    }
}