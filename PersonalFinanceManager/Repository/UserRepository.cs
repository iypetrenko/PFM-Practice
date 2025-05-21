using System.Linq;
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

        public User CheckLogin(string username, string password)
        {
            using (var db = GetContext())
            {
                var result = db.Users.FirstOrDefault(p => p.UserName == username && p.Password == password);
                return result;
            }
        }

        public bool RegisterUser(string username, string password)
        {
            using (var db = GetContext())
            {
                var user = new User
                {
                    UserName = username,
                    Password = password
                };
                db.Users.Add(user);
                var result = db.SaveChanges();
                return result > 0;
            }
        }

    }
}