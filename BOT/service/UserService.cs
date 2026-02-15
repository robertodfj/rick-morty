using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Bot.model.request;

namespace Bot.service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public UserService(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<string> GetMyUserInfo(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/user/my-info");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API error {response.StatusCode}: {content}");
            return content;
        }

        public async Task<string> Work(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/user/work");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API error {response.StatusCode}: {content}");
            return content;
        }

        public async Task<string> GetUserInfo(string token, string username)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/user/info/{username}");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"API error {response.StatusCode}: {content}");
            return content;
        }
    }
}