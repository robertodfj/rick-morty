using Bot.service;

namespace Bot.commands
{
    public class BuyCharacterCommand
    {
        private readonly TradeService _tradeService;

        public BuyCharacterCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // Ahora recibe directamente el int
        public async Task<(bool Success, string Message)> ExecuteAsync(int characterId, string token)
        {
            var result = await _tradeService.BuyCharacter(characterId, token);
            return (true, result);
        }
    }
}