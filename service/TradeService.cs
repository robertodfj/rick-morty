using RickYMorty.data;
using RickYMorty.dto;
using Microsoft.EntityFrameworkCore;

namespace RickYMorty.service
{
    public class TradeService
    {
        private readonly AppDBContext _context;

        public TradeService(AppDBContext context)
        {
            _context = context;
        }

        // Ver tienda de characters y episodes
        public async Task<List<TradeResponse>> GetCharactersForSale()
        {
            var charactersForSale = await _context.Characters.Where(c => c.ForSale).ToListAsync();
            var episodesForSale = await _context.Episodes.Where(e => e.ForSale).ToListAsync();

            var items = charactersForSale.Select(c => new TradeResponse
            {
                Id = c.Id,
                Name = c.Name,
                Type = "Character",
                Price = c.Price,
                ForSale = c.ForSale,
                ExtraInfo = c.Status // o Species, o cualquier info que quieras
            }).ToList<TradeResponse>();

            items.AddRange(episodesForSale.Select(e => new TradeResponse
            {
                Id = e.Id,
                Name = e.Name,
                Type = "Episode",
                Price = e.Price,
                ForSale = e.ForSale,
                Url = e.Url,
                Characters = e.Characters,
                ExtraInfo = e.EpisodeCode
            }));

            return items;
        }

        // Poner a la venta un personaje o episodio 
        public async Task<string> PutCharacterForSale(int userId, int characterId, double price)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.OwnedByUserId == userId);
            if (price == 0)
            {
                throw new Exception("Price must be greater than 0.");
            }
            if (character == null)
            {
                throw new Exception("Character not found or you do not own this character.");
            }

            if (character.ForSale)
            {
                throw new Exception("Character is already for sale.");
            }

            character.ForSale = true;
            character.Price = price;
            await _context.SaveChangesAsync();

            return $"Character with ID {characterId} is now for sale at price {price}.";
        }

        public async Task<string> PutEpisodeForSale(int userId, int episodeId, double price)
        {
            var episode = await _context.Episodes.FirstOrDefaultAsync(e => e.Id == episodeId && e.OwnedByUserId == userId);
            if (price == 0)
            {
                throw new Exception("Price must be greater than 0.");
            }
            if (episode == null)
            {
                throw new Exception("Episode not found or you do not own this episode.");
            }
            if (episode.ForSale)
            {
                throw new Exception("Episode is already for sale.");
            }

            episode.ForSale = true;
            episode.Price = price;
            await _context.SaveChangesAsync();

            return $"Episode with ID {episodeId} is now for sale at price {price}.";
        }

        // Comporar un personaje o episodio
        public async Task<CharacterResponse> BuyCharacter(int buyerId, int characterId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == buyerId);
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId && c.ForSale);
            if (user == null)
            {
                throw new Exception("Buyer not found.");
            }
            if (character == null)
            {
                throw new Exception("Character not found or not for sale.");
            }
            if (user.Money < character.Price)
            {
                throw new Exception("Buyer does not have enough money.");
            }
            if (character.OwnedByUserId == buyerId)
            {
                throw new Exception("You cannot buy your own character.");
            }
            user.Money -= character.Price;
            character.OwnedByUserId = buyerId;
            character.ForSale = false;
            character.Price = 0;
            await _context.SaveChangesAsync();
            return new CharacterResponse
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Gender = character.Gender,
                Price = character.Price,
                ForSale = character.ForSale
            };
        }

        public async Task<EpisodeResponse> BuyEpisode(int buyerId, int episodeId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == buyerId);
            var episode = await _context.Episodes.FirstOrDefaultAsync(e => e.Id == episodeId && e.ForSale);
            if (user == null)
            {
                throw new Exception("Buyer not found.");
            }
            if (episode == null)
            {
                throw new Exception("Episode not found or not for sale.");
            }
            if (user.Money < episode.Price)
            {
                throw new Exception("Buyer does not have enough money.");
            }
            if (episode.OwnedByUserId == buyerId)
            {
                throw new Exception("You cannot buy your own episode.");
            }
            user.Money -= episode.Price;
            episode.OwnedByUserId = buyerId;
            episode.ForSale = false;
            episode.Price = 0;
            await _context.SaveChangesAsync();
            return new EpisodeResponse
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
        }
    }
}