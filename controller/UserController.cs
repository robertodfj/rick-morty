using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RickYMorty.service;

namespace RickYMorty.controller
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("my-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            string username = User.Claims.First(c => c.Type == "Username").Value;
            var response = await userService.GetUserInfo(username);
            logger.LogInformation("User info retrieved successfully for user {Username}", username);
            return Ok(response);
        }
        [HttpGet("info/{username}")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            var response = await userService.GetUserInfo(username);
            logger.LogInformation("User info retrieved successfully for user {Username}", username);
            return Ok(response);
        }

        [HttpGet("work")]
        [Authorize]
        public async Task<IActionResult> Work()
        {
            int id = int.Parse(User.Claims.First(c => c.Type == "ID").Value);
            var response = await userService.Working(id);
            logger.LogInformation("User {ID} worked and earned {Money} money", id, response);
            return Ok(new { EarnedMoney = response });
        }
        
    }
}