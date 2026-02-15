using System.Security.Claims;
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
            var usernameClaim = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(usernameClaim))
                return Unauthorized("Username claim not found.");
            var response = await userService.GetUserInfo(usernameClaim);
            logger.LogInformation("User info retrieved successfully for user {Username}", usernameClaim);
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
            var idClaim = GetUserID();
            var response = await userService.Working(idClaim);
            logger.LogInformation("User {ID} worked and earned {Money} money", idClaim, response);
            return Ok(new { EarnedMoney = response });
        }

        [HttpPut("edit/{newUsername}")]
        [Authorize]
        public async Task<IActionResult> EditUsername(string newUsername)
        {
            var idClaim = GetUserID();
            await userService.EditUsername(idClaim, newUsername);
            logger.LogInformation("User {ID} changed username to {NewUsername}", idClaim, newUsername);
            return Ok(new { Message = "Username updated successfully" });
        }

        private int GetUserID()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idClaim))
                throw new UnauthorizedAccessException("User ID claim not found.");
            return int.Parse(idClaim);
        }
        
    }
}