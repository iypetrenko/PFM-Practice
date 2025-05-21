using System.Linq;
using System.Windows;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager.Repository
{
    public class UserRepository : IUserRepository
    {
        protected virtual PersonalFinanceManagerContext GetContext()
        {
            return new PersonalFinanceManagerContext();
        }

        public IEnumerable<User> GetAdministrators()
        {
            using (var db = GetContext()) // Fix: Use the GetContext method to create a context instance
            {
                return db.Users.Where(u => u.Role == Model.UserRole.Admin).ToList();
            }
        }

        public User CheckLogin(string username, string password)
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                // Автоматическое создание администратора при первом входе
                if (username == "admin" && password == "admin")
                {
                    var admin = context.Users.FirstOrDefault(u => u.UserName == "admin");
                    if (admin == null)
                    {
                        admin = new User
                        {
                            UserName = "admin",
                            Password = "admin",
                            Role = UserRole.Admin
                        };
                        context.Users.Add(admin);
                        context.SaveChanges();
                    }
                    return admin;
                }

                return context.Users
                    .FirstOrDefault(u => u.UserName == username && u.Password == password);
            }
        }
        public bool AdminExists()
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                return context.Users.Any(u => u.Role == UserRole.Admin);
            }
        }

        public void CreateAdminIfNotExists()
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                if (!context.Users.Any(u => u.Role == UserRole.Admin))
                {
                    var admin = new User
                    {
                        UserName = "admin",
                        Password = "admin",
                        Role = UserRole.Admin
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();
                }
            }
        }
        public bool RegisterUser(string username, string password)
        {
            using (var db = GetContext())
            {
                if (db.Users.Any(u => u.UserName == username))
                {
                    MessageBox.Show("Користувач з таким ім'ям вже існує!");
                    return false;
                }

                var user = new User
                {
                    UserName = username,
                    Password = password,
                    Role = UserRole.User
                };

                db.Users.Add(user);
                return db.SaveChanges() > 0;
            }
        }
        public User CreateGuestUser()
        {
            return new User
            {
                UserName = "Guest_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                Password = "guest_temp_password",
                Role = UserRole.Guest
            };
        }
        public List<User> GetAllUsers()
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                return context.Users.ToList();
            }
        }

        public bool UpdateUser(User user)
        {
            using (var context = new PersonalFinanceManagerContext())
            {
                var existingUser = context.Users.Find(user.Id);
                if (existingUser == null) return false;

                existingUser.UserName = user.UserName;
                existingUser.Role = user.Role;
                return context.SaveChanges() > 0;
            }
        }
    }
}