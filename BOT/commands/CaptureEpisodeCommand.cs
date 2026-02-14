using Bot.service;

namespace Bot.commands
{
    public class CaptureEpisodeCommand
    {
        private readonly TradeService _tradeService;

        public CaptureEpisodeCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token)
        {
            var episode = await _tradeService.GetRandomEpisode(token);
            return (true, episode);
        }
    }
}