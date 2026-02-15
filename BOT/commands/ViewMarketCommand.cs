using Bot.service;

namespace Bot.commands
{
    public class ViewMarketCommand
    {
        private readonly TradeService _tradeService;

        public ViewMarketCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // Ahora recibe directamente el int
        public async Task<(bool Success, string Message)> ExecuteAsync(string token)
        {
            var result = await _tradeService.ViewMarket(token);
            return (true, result);
        }
    }
}