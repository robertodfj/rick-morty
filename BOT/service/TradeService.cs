using Bot.model.request;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace Bot.service
{
    public class TradeService
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public TradeService(HttpClient httpClient, string apiUrl)
        {
            _httpClient = httpClient;
            _apiUrl = apiUrl;
        }

        public async Task<string> GetRandomCharacter(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/characters/capture");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetRandomEpisode(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/episodes/capture");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> PutCharacterForSale(ItemForSaleRequest itemForSaleRequest, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/characters/put-for-sale")
            {
                Content = JsonContent.Create(itemForSaleRequest)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> PutEpisodeForSale(ItemForSaleRequest itemForSaleRequest, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/episodes/put-for-sale")
            {
                Content = JsonContent.Create(itemForSaleRequest)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> BuyCharacter(int characterId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/characters/buy/{characterId}")
            {
                Content = new StringContent(characterId.ToString(), System.Text.Encoding.UTF8, "text/plain")
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> BuyEpisode(int episodeId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_apiUrl.TrimEnd('/')}/episodes/buy/{episodeId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> ViewMarket(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/characters/for-sale");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetMyCharacters(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/characters/my-characters");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<string> GetMyEpisodes(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl.TrimEnd('/')}/episodes/my-episodes");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}