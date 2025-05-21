using System.Data.Entity;
using PersonalFinanceManager.Migrations;
using PersonalFinanceManager.Model;

public class PersonalFinanceManagerContext : DbContext
{
    public PersonalFinanceManagerContext()
        : base("DefaultConnection")
    {
        Database.SetInitializer(new MigrateDatabaseToLatestVersion<PersonalFinanceManagerContext, Configuration>());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<Item> Items { get; set; }
}