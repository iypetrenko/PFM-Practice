using PersonalFinanceManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task AddTransaction(Transaction transaction);
        Task DeleteTransaction(int id);
    }
}