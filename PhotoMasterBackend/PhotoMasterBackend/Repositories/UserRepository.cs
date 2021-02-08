using Microsoft.EntityFrameworkCore;
using PhotoMasterBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PhotoContext _context;

        public UserRepository(PhotoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<User> AddUserAsync(User user)
        {
            var result = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var result = await _context.Users.FindAsync(user.Id);

            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Username = user.Username;
                result.Password = user.Password;
                result.Role = user.Role;
                result.Token = user.Token;
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var result = await _context.Users.FindAsync(userId);
            if (result != null)
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
    }
}