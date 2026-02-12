using Microsoft.EntityFrameworkCore;
using RickYMorty.data;
using RickYMorty.dto;
using RickYMorty.middleware;

namespace RickYMorty.service
{
    public class CharacterService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDBContext _context;

        public CharacterService(HttpClient httpClient, AppDBContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<CharacterResponse> CaptureCharacter(int ownerId)
        {
            int characterId = Random.Shared.Next(1, 827); // Genera un ID aleatorio entre 1 y 827
            var user = await _context.Users.FindAsync(ownerId);
            var existing = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId);

            int totalCharacters = 826; // Total de personajes disponibles en la API
            var capturedCount = await _context.Characters.CountAsync(); 
            if (capturedCount >= totalCharacters) 
            { 
                throw new ConflictException("All characters have been captured. Try again later!"); 
            }

            // Hacer que genere un numero que no este ya capturado
            while (existing != null) 
            { 
                characterId = Random.Shared.Next(1, 827); existing = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId); 
            }
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (!CaptureSuccess(user.TimesWorked ?? 0))
            {
                throw new ConflictException("Capture failed. Keep working to increase your chances!");
            }

            var response = await _httpClient.GetFromJsonAsync<CharacterResponse>($"https://rickandmortyapi.com/api/character/{characterId}");
            if (response == null)
            {
                throw new NotFoundException("Character not found");
            }

            var character = new model.Character
            {
                Name = response.Name,
                Status = response.Status,
                Species = response.Species,
                Gender = response.Gender,
                ForSale = false,
                Price = 0,
                OwnedByUserId = ownerId
            };

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            var characterResponse = new CharacterResponse
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Gender = character.Gender,
                ForSale = character.ForSale,
                Price = character.Price
            };

            return characterResponse;

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