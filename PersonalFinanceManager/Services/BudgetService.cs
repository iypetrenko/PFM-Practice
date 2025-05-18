using PersonalFinanceManager.Data;
using PersonalFinanceManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace PersonalFinanceManager.Services
{
    public class BudgetService
    {
        private readonly ApplicationDbContext _context;

        public BudgetService()
        {
            _context = new ApplicationDbContext();
        }

        public void AddBudget(Budget budget)
        {
            _context.Budgets.Add(budget);
            _context.SaveChanges();
        }

        public List<Budget> GetBudgets()
        {
            return _context.Budgets.ToList();
        }

        public decimal GetSpentForCategory(int categoryId)
        {
            return _context.Transactions
                .Where(t => t.CategoryId == categoryId && t is ExpenseTransaction)
                .Sum(t => t.Amount);
        }

    }
}