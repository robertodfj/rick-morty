using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class PutEpisodeForSaleCommand
    {
        private readonly TradeService _tradeService;

        public PutEpisodeForSaleCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(ItemForSaleRequest itemForSaleRequest, string token)
        {
            var result = await _tradeService.PutEpisodeForSale(itemForSaleRequest, token);
            return (true, result);
        }
    }
}