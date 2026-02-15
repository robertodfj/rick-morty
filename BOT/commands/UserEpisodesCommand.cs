using Bot.service;

namespace Bot.commands
{
    public class UserEpisodesCommand
    {
        private readonly TradeService _tradeService;

        public UserEpisodesCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token, string username)
        {
            var result = await _tradeService.GetUserEpisodes(token, username);
            return (true, result);
        }
    }
}