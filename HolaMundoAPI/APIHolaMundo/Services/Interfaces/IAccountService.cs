using DB.Data.Models;

namespace APIHolaMundo.Services.Interfaces
{
    public interface IAccountService
    {
        string GenerateJwtToken(User user);

    }
}
