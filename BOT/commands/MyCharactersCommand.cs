using Bot.service;

namespace Bot.commands
{
    public class MyCharactersCommand
    {
        private readonly TradeService _tradeService;

        public MyCharactersCommand(TradeService tradeService)
        {
            _tradeService = tradeService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token)
        {
            var result = await _tradeService.GetMyCharacters(token);
            return (true, result);
        }
    }
}