using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с финансовыми транзакциями
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Добавляет новую транзакцию
        /// </summary>
        /// <param name="transaction">Транзакция для добавления</param>
        /// <returns>Идентификатор добавленной транзакции</returns>
        int AddTransaction(Models.Transaction transaction);

        /// <summary>
        /// Удаляет существующую транзакцию
        /// </summary>
        /// <param name="transactionId">Идентификатор транзакции для удаления</param>
        /// <returns>True, если удаление выполнено успешно</returns>
        bool RemoveTransaction(int transactionId);

        /// <summary>
        /// Обновляет информацию о транзакции
        /// </summary>
        /// <param name="transaction">Обновленная транзакция</param>
        /// <returns>True, если обновление выполнено успешно</returns>
        bool UpdateTransaction(Models.Transaction transaction);

        /// <summary>
        /// Получает транзакцию по её идентификатору
        /// </summary>
        /// <param name="transactionId">Идентификатор транзакции</param>
        /// <returns>Объект транзакции</returns>
        Models.Transaction GetTransaction(int transactionId);

        /// <summary>
        /// Получает список транзакций в указанном диапазоне дат
        /// </summary>
        /// <param name="startDate">Начальная дата диапазона</param>
        /// <param name="endDate">Конечная дата диапазона</param>
        /// <returns>Список транзакций</returns>
        IEnumerable<Models.Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Получает список транзакций по указанной категории
        /// </summary>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <returns>Список транзакций</returns>
        IEnumerable<Models.Transaction> GetTransactionsByCategory(int categoryId);

        /// <summary>
        /// Получает список транзакций по указанному счету
        /// </summary>
        /// <param name="accountId">Идентификатор счета</param>
        /// <returns>Список транзакций</returns>
        IEnumerable<Models.Transaction> GetTransactionsByAccount(int accountId);

        /// <summary>
        /// Асинхронно получает список всех транзакций пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Задача, результатом которой является список транзакций</returns>
        Task<IEnumerable<Models.Transaction>> GetAllUserTransactionsAsync(int userId);
    }
}