using Bot.service;

namespace Bot.commands
{
    public class UserCharactersCommand
    {
        private readonly TradeService _tradeService;

        public UserCharactersCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token, string username)
        {
            var result = await _tradeService.GetUserCharacters(token, username);
            return (true, result);
        }
    }
}