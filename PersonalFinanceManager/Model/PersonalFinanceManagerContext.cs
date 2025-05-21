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
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseCategory>()
            .HasMany(ec => ec.Items)
            .WithRequired(i => i.ExpenseCategory)
            .HasForeignKey(i => i.ToDoListId);
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public virtual DbSet<Item> Items { get; set; }
}