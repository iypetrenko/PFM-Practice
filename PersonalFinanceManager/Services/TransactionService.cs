using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services
{
    public class TransactionService : ITransactionService
    {
        // Временное хранилище в памяти (замените на реальную БД)
        private readonly List<Transaction> _transactions = new();
        private int _nextId = 1;

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            // Имитация асинхронной операции
            return await Task.FromResult(_transactions.AsReadOnly());
        }

        public async Task AddTransaction(Transaction transaction)
        {
            transaction.Id = _nextId++;
            _transactions.Add(transaction);
            await Task.CompletedTask;
        }

        public async Task DeleteTransaction(int id)
        {
            var transaction = _transactions.Find(t => t.Id == id);
            if (transaction != null)
            {
                _transactions.Remove(transaction);
            }
            await Task.CompletedTask;
        }
    }
}