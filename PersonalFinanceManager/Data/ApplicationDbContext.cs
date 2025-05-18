using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=finance.db");
    }
}