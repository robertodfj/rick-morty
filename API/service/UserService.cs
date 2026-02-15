using RickYMorty.data;
using RickYMorty.dto;
using RickYMorty.model;
using Microsoft.EntityFrameworkCore;
using RickYMorty.middleware;
using RickYMorty.token;

namespace RickYMorty.service
{
    public class UserService
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        public async Task<UserInfoResponse> GetUserInfo(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                throw new NotFoundException("User not found");


            return new UserInfoResponse
            {
                Username = user.Username,
                Money = user.Money,
                LastWorked = user.LastWorked
            };
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
                    throw new ConflictException($"You have already worked recently. Please wait {minutesLeft} more minutes before working again.");
                }
            }
            user.LastWorked = DateTime.Now;
            user.Money += 100;
            await _context.SaveChangesAsync();
            return $"User {user.Username} has worked and earned 100 money. Total money: {user.Money}";
        }

        public async Task<string> EditUsername(int id, string newUsername)
        {
            var user = await GetUserByID(id);
            if (newUsername == user.Username)
            {
                throw new ConflictException("New username cannot be the same as the current username");
            }
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                throw new BadRequestException("Username cannot be empty");
            }
            if (await _context.Users.AnyAsync(u => u.Username == newUsername))
            {
                throw new ConflictException("Username already exists");
            }
            user.Username = newUsername;
            await _context.SaveChangesAsync();
            return new GenerateToken().generateToken(user, _configuration);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task<User> GetUserByID(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }
    }
}