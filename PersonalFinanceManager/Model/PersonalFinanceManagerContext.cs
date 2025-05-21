using System.Data.Entity;

namespace PersonalFinanceManager.Model
{
    public class PersonalFinanceManagerContext : DbContext
    {
        public PersonalFinanceManagerContext() : base("PersonalFinanceManagerContext")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<PersonalFinanceManagerContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}