using Microsoft.EntityFrameworkCore;
using RickYMorty.data;
using RickYMorty.dto;
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

        public async Task<EpisodeResponse> CaptureEpisode(GetEpisode getEpisode)
        {
            var user = await _context.Users.FindAsync(getEpisode.OwnerID);
            var existing = await _context.Episodes.FirstOrDefaultAsync(e => e.Id == getEpisode.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (getEpisode.Id <= 0)
            {
                throw new Exception("Invalid episode ID");
            }
            if (existing != null)
            {
                throw new Exception($"Episode already owned by user {existing.OwnedByUserId}");
            }
            if (!CaptureSuccess(user.TimesWorked ?? 0))
            {
                throw new Exception("Capture failed. Keep working to increase your chances!");
            }

            var response = await _httpClient.GetFromJsonAsync<EpisodeResponse>($"https://rickandmortyapi.com/api/episode/{getEpisode.Id}");
            if (response == null)
            {
                throw new Exception("Episode not found");
            }

            var episode = new Episode
            {
                Id = response.Id,
                Name = response.Name,
                AirDate = response.AirDate,
                EpisodeCode = response.Episode,
                Characters = response.Characters,
                Url = response.Url ?? string.Empty,
                Created = response.Created ?? string.Empty,
                ForSale = false,
                Price = 0,
                OwnedByUserId = getEpisode.OwnerID
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