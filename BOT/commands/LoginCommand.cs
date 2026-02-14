using System.Threading.Tasks;
using Bot.service;
using Bot.model.request;

namespace Bot.commands
{
    public class LoginCommand
    {
        private readonly AuthService _authService;

        public LoginCommand(AuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> ExecuteAsync(LoginRequest loginRequest)
        {
            return await _authService.Login(loginRequest);
        }
    }
}