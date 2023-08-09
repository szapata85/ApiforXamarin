using APIHolaMundo.Services.Interfaces;
using DB.Data.Dto;
using DB.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIHolaMundo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public AccountController(IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            User user = await _userService.GetUserAsync(userName, password);

            if (user == null)
            {
                return Unauthorized("Usuario o contraseña inválidos");
            }

            var token = _accountService.GenerateJwtToken(user);

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Role = user.Role,
                Token = token
            };

            return Ok(userDto);
        }

        // Otros métodos para SignIn, Logout, ForgotPassword, etc.
    }
}
