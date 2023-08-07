using DB.Data.Models;

namespace Services.Services.Interfaces
{
    public interface IUserService
    {
        Task<User>? GetUserAsync(string username, string password);

    }
}
