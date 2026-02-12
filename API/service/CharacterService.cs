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

        public async Task<CharacterResponse> CaptureCharacter(GetCharacterDTO getCharacterDTO, int ownerId)
        {
            var user = await _context.Users.FindAsync(ownerId);
            var existing = await _context.Characters.FirstOrDefaultAsync(c => c.Id == getCharacterDTO.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (getCharacterDTO.Id <= 0)
            {
                throw new BadRequestException("Invalid character ID");
            }
            if (existing != null)
            {
                throw new ConflictException($"Character already owned by user {existing.OwnedByUserId}");
            }
            if (!CaptureSuccess(user.TimesWorked ?? 0))
            {
                throw new ConflictException("Capture failed. Keep working to increase your chances!");
            }

            var response = await _httpClient.GetFromJsonAsync<CharacterResponse>($"https://rickandmortyapi.com/api/character/{getCharacterDTO.Id}");
            if (response == null)
            {
                throw new NotFoundException("Character not found");
            }

            var character = new model.Character
            {
                Id = response.Id,
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