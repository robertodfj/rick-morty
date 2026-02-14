using System.Threading.Tasks;
using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class RegisterCommand
    {
        private readonly AuthService _authService;

        public RegisterCommand(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Execute(RegisterRequest registerRequest)
        {
            return await _authService.Register(registerRequest);
        }
    }
}