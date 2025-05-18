using PersonalFinanceManager.Data;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService()
        {
            _context = new ApplicationDbContext();
        }

        public bool Login(string username, string password)
        {
            return _context.Users.Any(u => u.Username == username && u.Password == password);
        }
    }
}