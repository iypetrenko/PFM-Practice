using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Data;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Services
{
    public class TransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Transaction> GetTransactions()
        {
            return _context.Transactions
                .Include(t => t.CategoryId) // Replace 'Category' with 'CategoryId'
                .ToList();
        }

        public void AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }
    }
}