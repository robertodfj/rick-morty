using Microsoft.EntityFrameworkCore;
using RickYMorty.data;
using RickYMorty.dto;
using RickYMorty.middleware;
using RickYMorty.model;

namespace RickYMorty.service
{
    public class EpisodeService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDBContext _context;

        public EpisodeService(HttpClient httpClient, AppDBContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<EpisodeResponse> CaptureEpisode(int ownerId)
        {
            var user = await _context.Users.FindAsync(ownerId);
            int episodeId = Random.Shared.Next(1, 51); // Genera un ID aleatorio entre 1 y 51

            int totalEpisodes = 51;
            var capturedCount = await _context.Episodes.CountAsync(); 
            if (capturedCount >= totalEpisodes) 
            { 
                throw new ConflictException("All episodes have been captured. Try again later!"); 
            }
            
            var existing = await _context.Episodes.FirstOrDefaultAsync(e => e.Id == episodeId);
            while (existing != null)
            {
                episodeId = Random.Shared.Next(1, 51);
                existing = await _context.Episodes.FirstOrDefaultAsync(e => e.Id == episodeId);
            }
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (!CaptureSuccess(user.TimesWorked ?? 0))
            {
                throw new ConflictException("Capture failed. Keep working to increase your chances!");
            }

            var response = await _httpClient.GetFromJsonAsync<EpisodeResponse>($"https://rickandmortyapi.com/api/episode/{episodeId}");
            if (response == null)
            {
                throw new NotFoundException("Episode not found");
            }

            var episode = new Episode
            {
                Name = response.Name ?? "Unknown",
                AirDate = string.IsNullOrEmpty(response.AirDate) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : response.AirDate,
                EpisodeCode = response.Episode ?? "Unknown",
                Characters = response.Characters ?? Array.Empty<string>(),
                Url = response.Url ?? string.Empty,
                Created = string.IsNullOrEmpty(response.Created) ? DateTime.UtcNow.ToString("yyyy-MM-dd") : response.Created,
                ForSale = false,
                Price = 0,
                OwnedByUserId = ownerId
            };

            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync();

            var episodeResponse = new EpisodeResponse
            {
                Id = episode.Id,
                Name = episode.Name,
                AirDate = episode.AirDate,
                Episode = episode.EpisodeCode,
                Characters = episode.Characters,
                Url = episode.Url,
                Created = episode.Created,
                ForSale = episode.ForSale,
                Price = episode.Price
            };

            return episodeResponse;

        }

        public bool CaptureSuccess(int timesWorked)
        {
            // Probabilidad base (muy baja)
            int baseChance = 10;

            // Cada trabajo suma probabilidad
            int bonusPerWork = 5;

            int chance = baseChance + (timesWorked * bonusPerWork);

            // Capamos para que nunca sea seguro al 100%
            chance = Math.Min(chance, 90);

            int roll = Random.Shared.Next(1, 101); // 1..100

            return roll <= chance;
        }
    }
}