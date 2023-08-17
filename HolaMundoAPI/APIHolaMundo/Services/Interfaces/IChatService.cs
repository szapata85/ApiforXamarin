using DB.Data.Models;

namespace APIHolaMundo.Services.Interfaces
{
    public interface IChatService
    {
        Task<string> SendAsync(SendMessageInput input);
    }
}
