using APIHolaMundo.Services.Interfaces;
using DB.Data;
using DB.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace APIHolaMundo.Services
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
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.UserName == username && user.Password == password);

            return user;
        }
    }
}
