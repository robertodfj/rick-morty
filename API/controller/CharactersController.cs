using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RickYMorty.dto;
using RickYMorty.middleware;
using RickYMorty.service;

namespace RickYMorty.controller
{
    [ApiController]
    [Route("characters")]
    public class CharactersController : ControllerBase
    {
        private readonly CharacterService characterService;
        private readonly TradeService tradeService;
        private readonly UserService userService;
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(CharacterService characterService, TradeService tradeService, UserService userService, ILogger<CharactersController> logger)
        {
            this.characterService = characterService;
            this.tradeService = tradeService;
            this.userService = userService;
            this._logger = logger;
        }

        // Capturar un personaje
        [HttpPost("capture")]
        [Authorize]
        public async Task<IActionResult> CaptureCharacter()
        {
            var ownerId = GetUserID();
            var characterResponse = await characterService.CaptureCharacter(ownerId);
            _logger.LogInformation("Character captured successfully by user {UserId} with character ID {CharacterId}", ownerId, characterResponse.Id);
            return Ok(characterResponse);
        }


        // Ver mis personajes
        [HttpGet("my-characters")]
        [Authorize]
        public async Task<IActionResult> MyCharacters()
        {
            var username = GetUsername();
            var characters = await userService.GetUserCharacters(username);
            return Ok(characters);
        }

        // Ver personajes de un usuario
        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> GetUserCharacters(string username)
        {
            var characters = await userService.GetUserCharacters(username);
            return Ok(characters);
        }

        // Ver personajes y episodios a la venta
        [HttpGet("for-sale")]
        [Authorize]
        public async Task<IActionResult> GetForSale()
        {
            var itemsForSale = await tradeService.GetForSale();
            return Ok(itemsForSale);
        }

        // Poner un personaje a la venta
        [HttpPost("put-for-sale")]
        [Authorize]
        public async Task<IActionResult> PutCharacterForSale([FromBody] PutItemForSaleDTO putCharacterForSale)
        {
            var userId = GetUserID();
            var result = await tradeService.PutCharacterForSale(userId, putCharacterForSale);
            _logger.LogInformation("Character with ID {CharacterId} put for sale by user {UserId} at price {Price}", putCharacterForSale.ItemId, userId, putCharacterForSale.Price);
            return Ok(result);
        }

        // Comprar un personaje
        [HttpPost("buy")]
        [Authorize]
        public async Task<IActionResult> BuyCharacter([FromBody] int characterId)
        {
            var buyerId = GetUserID();
            var result = await tradeService.BuyCharacter(buyerId, characterId);
            _logger.LogInformation("Character with ID {CharacterId} bought by user {UserId}", characterId, buyerId);
            return Ok(result);
        }


        // Obtener ID del usuario autenticado
        private int GetUserID()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedException("User ID claim not found.");

            return int.Parse(userIdClaim);
        }

        // Obtener username del usuario autenticado
        private string GetUsername()
        {
            var usernameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(usernameClaim))
                throw new UnauthorizedException("Username claim not found.");

            return usernameClaim;
        }
    }
}