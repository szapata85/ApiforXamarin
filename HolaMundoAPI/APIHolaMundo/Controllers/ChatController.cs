using APIHolaMundo.Services.Interfaces;
using DB.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIHolaMundo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] SendMessageInput input)
        {
            return Ok(await _chatService.SendAsync(input));
        }
    }
}
