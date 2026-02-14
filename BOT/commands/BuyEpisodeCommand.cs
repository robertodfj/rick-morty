using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class BuyEpisodeCommand
    {
        private readonly TradeService _tradeService;

        public BuyEpisodeCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(int episodeID, string token)
        {
            var result = await _tradeService.BuyEpisode(episodeID, token);
            return (true, result);
        }
    }
}