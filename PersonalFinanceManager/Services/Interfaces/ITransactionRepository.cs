using PersonalFinanceManager.Models; // Add this namespace import

namespace PersonalFinanceManager.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с финансовыми транзакциями
    /// </summary>
    public interface ITransactionRepository
    {
        int AddTransaction(Transaction transaction); // Removed redundant 'Models.' prefix
        bool RemoveTransaction(int transactionId);
        bool UpdateTransaction(Transaction transaction); // Removed redundant 'Models.' prefix
        Transaction GetTransaction(int transactionId); // Removed redundant 'Models.' prefix
        IEnumerable<Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate); // Removed redundant 'Models.' prefix
        IEnumerable<Transaction> GetTransactionsByCategory(int categoryId); // Removed redundant 'Models.' prefix
        IEnumerable<Transaction> GetTransactionsByAccount(int accountId); // Removed redundant 'Models.' prefix
        Task<IEnumerable<Transaction>> GetAllUserTransactionsAsync(int userId); // Removed redundant 'Models.' prefix
    }
}
