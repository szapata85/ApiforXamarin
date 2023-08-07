using DB.Data.Models;

namespace APIHolaMundo.Services.Interfaces
{
    public interface IUserService
    {
        Task<User>? GetUserAsync(string username, string password);

    }
}
