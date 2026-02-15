using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RickYMorty.dto;
using RickYMorty.middleware;
using RickYMorty.service;
using System.Security.Claims;

namespace RickYMorty.controller
{
    [ApiController]
    [Route("episodes")]
    public class EpisodesController : ControllerBase
    {
        private readonly EpisodeService episodeService;
        private readonly TradeService tradeService;
        private readonly UserService userService;
        private readonly ILogger<EpisodesController> _logger;

        public EpisodesController(EpisodeService episodeService, TradeService tradeService, UserService userService, ILogger<EpisodesController> logger)
        {
            this.episodeService = episodeService;
            this.tradeService = tradeService;
            this.userService = userService;
            this._logger = logger;
        }

        // Capturar un episodio
        [HttpPost("capture")]
        [Authorize]
        public async Task<IActionResult> CaptureEpisode()
        {
            var ownerId = GetUserID();
            var episodeResponse = await episodeService.CaptureEpisode(ownerId);
            _logger.LogInformation("Episode captured successfully by user {UserId} with episode ID {EpisodeId}", ownerId, episodeResponse.Id);
            return Ok(episodeResponse);
        }

        // Ver mis episodios
        [HttpGet("my-episodes")]
        [Authorize]
        public async Task<IActionResult> MyEpisodes()
        {
            var username = GetUsername();
            var episodes = await userService.GetUserEpisodes(username);
            return Ok(episodes);
        }

        // Ver episodios de un usuario
        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> GetUserEpisodes(string username)
        {
            var episodes = await userService.GetUserEpisodes(username);
            return Ok(episodes);
        }

        // Ver personajes y episodios a la venta
        [HttpGet("for-sale")]
        [Authorize]
        public async Task<IActionResult> GetForSale()
        {
            var itemsForSale = await tradeService.GetForSale();
            return Ok(itemsForSale);
        }

        // Poner un episodio a la venta
        [HttpPost("put-for-sale")]
        [Authorize]
        public async Task<IActionResult> PutEpisodeForSale([FromBody] PutItemForSaleDTO putEpisodeForSale)
        {
            var userId = GetUserID();
            var result = await tradeService.PutEpisodeForSale(userId, putEpisodeForSale);
            _logger.LogInformation("Episode with ID {EpisodeId} put for sale by user {UserId} at price {Price}", putEpisodeForSale.ItemId, userId, putEpisodeForSale.Price);
            return Ok(result);
        }

        // Quitar un episodio de la venta
        [HttpDelete("remove-from-sale/{episodeId}")]
        [Authorize]
        public async Task<IActionResult> RemoveEpisodeFromSale(int episodeId)
        {
            var userId = GetUserID();
            var result = await tradeService.RemoveEpisodeFromSale(userId, episodeId);
            _logger.LogInformation("Episode with ID {EpisodeId} removed from sale by user {UserId}", episodeId, userId);
            return Ok(result);
        }

        // Comprar un episodio
        [HttpPost("buy/{episodeId}")]
        [Authorize]
        public async Task<IActionResult> BuyEpisode(int episodeId)
        {
            var buyerId = GetUserID();
            var result = await tradeService.BuyEpisode(buyerId, episodeId);
            _logger.LogInformation("Episode with ID {EpisodeId} bought by user {UserId}", episodeId, buyerId);
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