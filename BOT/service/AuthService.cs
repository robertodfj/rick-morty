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

        public async Task<string> Register(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/register", registerRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> RegisterAdmin(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/register-admin", registerRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiUrl.TrimEnd('/')}/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}