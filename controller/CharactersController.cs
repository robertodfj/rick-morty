using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RickYMorty.dto;
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
        public async Task<IActionResult> CaptureCharacter([FromBody] GetCharacterDTO getCharacterDTO)
        {
            int ownerId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value);
           var characterResponse = await characterService.CaptureCharacter(getCharacterDTO, ownerId);

           _logger.LogInformation("Character captured successfully by user {UserId} with character ID {CharacterId}", ownerId, getCharacterDTO.Id);
           
           return Ok(characterResponse);
        }


        // Ver mis personajes
        [HttpGet("my-characters")]
        [Authorize]
        public async Task<IActionResult> MyCharacters()
        {
            string username = User.Claims.First(c => c.Type == "Username").Value;
            var characters = await userService.GetUserByUsername(username);
            return Ok(characters);
        }

        // Ver personajes de un usuario
        [HttpGet("user-characters")]
        [Authorize]
        public async Task<IActionResult> GetUserCharacters([FromBody] string username)
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
        public async Task<IActionResult> PutCharacterForSale([FromBody] ItemForSale putCharacterForSale)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value);
            var result = await tradeService.PutCharacterForSale(userId, putCharacterForSale);
            _logger.LogInformation("Character with ID {CharacterId} put for sale by user {UserId} at price {Price}", putCharacterForSale.ItemId, userId, putCharacterForSale.Price);
            return Ok(result);
        }
    }
}