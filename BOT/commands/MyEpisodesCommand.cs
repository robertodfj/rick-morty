using Bot.service;

namespace Bot.commands
{
    public class MyEpisodesCommand
    {
        private readonly TradeService _tradeService;

        public MyEpisodesCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token)
        {
            var result = await _tradeService.GetMyEpisodes(token);
            return (true, result);
        }
    }
}