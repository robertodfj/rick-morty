using Bot.model.request;
using System.Net.Http.Json;

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
    }
}