using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class PutCharacterForSaleCommand
    {
        private readonly TradeService _tradeService;

        public PutCharacterForSaleCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(ItemForSaleRequest itemForSaleRequest, string token)
        {
            var result = await _tradeService.PutCharacterForSale(itemForSaleRequest, token);
            return (true, result);
        }

        public async Task<(bool Success, string Message)> CancelAsync(int characterId, string token)
        {
            var result = await _tradeService.CancelCharacterForSale(characterId, token);
            return (true, result);
        }
    }
}