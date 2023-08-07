using DB.Data;
using DB.Data.Models;
using Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;


        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User>? GetUserAsync(string username, string password)
        {
            if (_context.Users == null)
            {
                return null;
            }
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username && user.Password == password);

            return user;
        }
    }
}
