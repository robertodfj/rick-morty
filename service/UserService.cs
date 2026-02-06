using RickYMorty.data;
using RickYMorty.dto;
using RickYMorty.model;
using Microsoft.EntityFrameworkCore;

namespace RickYMorty.service
{
    public class UserService
    {
        private readonly AppDBContext _context;

        public UserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<CharacterResponse>> GetUserCharacters(string username)
        {
            // Buscar al usuario por username
            var user = await GetUserByUsername(username);
            // Obtener los personajes del usuario
            var characters = await _context.Characters.Where(c => c.OwnedByUserId == user.Id).ToListAsync();

            // Mapear a DTO
            var response = characters.Select(c => new CharacterResponse
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status,
                Species = c.Species,
                Gender = c.Gender
            }).ToList();

            return response;
        }

        public async Task<List<EpisodeResponse>> GetUserEpisodes(string username)
        {
            // Buscar al usuario por username
            var user = await GetUserByUsername(username);
            // Obtener los personajes del usuario
            var episodes = await _context.Episodes.Where(e => e.OwnedByUserId == user.Id).ToListAsync();

            // Mapear a DTO
            var response = episodes.Select(e => new EpisodeResponse
            {
                Id = e.Id,
                Name = e.Name,
                AirDate = e.AirDate,
                Episode = e.EpisodeCode,
                Characters = e.Characters,
                Url = e.Url,
                Created = e.Created
            }).ToList();

            return response;
        }

        public async Task<string> Working(int id)
        {
            var user = await GetUserByID(id);
            if (user.LastWorked.HasValue)
            {
               var timeSinceLastWork = DateTime.Now - user.LastWorked.Value;
                if (timeSinceLastWork.TotalMinutes < 15)
                {
                    var minutesLeft = 15 - (int)timeSinceLastWork.TotalMinutes;
                    throw new Exception($"You have already worked recently. Please wait {minutesLeft} more minutes before working again.");
                }
            }
            user.LastWorked = DateTime.Now;
            user.Money += 100;
            await _context.SaveChangesAsync();
            return $"User {user.Username} has worked and earned 100 money. Total money: {user.Money}";
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> GetUserByID(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }
    }
}