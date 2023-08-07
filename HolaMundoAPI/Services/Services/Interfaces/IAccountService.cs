using DB.Data.Models;

namespace Services.Services.Interfaces
{
    public interface IAccountService
    {
        string GenerateJwtToken(User user);

    }
}
