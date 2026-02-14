using System.Threading.Tasks;
using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class CaptureCharacterCommand
    {
        private readonly TraderService _traderService;

        public CaptureCharacterCommand(TraderService traderService)
        {
            _traderService = traderService;
        }

        public async Task<(bool Success, string Message)> ExecuteAsync(string token)
        {
            var character = await _traderService.GetRandomCharacter(token);
            return (true, character);
        }
    }
}