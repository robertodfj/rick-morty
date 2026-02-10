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
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(CharacterService characterService, TradeService tradeService, ILogger<CharactersController> logger)
        {
            this.characterService = characterService;
            this.tradeService = tradeService;
            this._logger = logger;
        }

        [HttpPost("capture")]
        [Authorize]
        public async Task<IActionResult> CaptureCharacter([FromBody] GetCharacterDTO getCharacterDTO)
        {
            int ownerId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value);
           var characterResponse = await characterService.CaptureCharacter(getCharacterDTO, ownerId);

           _logger.LogInformation("Character captured successfully by user {UserId} with character ID {CharacterId}", ownerId, getCharacterDTO.Id);
           
           return Ok(characterResponse);
        }


    }
}