using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для управления счетами пользователя
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Добавляет новый счет
        /// </summary>
        /// <param name="account">Счет для добавления</param>
        /// <returns>Идентификатор добавленного счета</returns>
        int AddAccount(Models.Account account);

        /// <summary>
        /// Удаляет существующий счет
        /// </summary>
        /// <param name="accountId">Идентификатор счета для удаления</param>
        /// <returns>True, если удаление выполнено успешно</returns>
        bool RemoveAccount(int accountId);

        /// <summary>
        /// Обновляет информацию о счете
        /// </summary>
        /// <param name="account">Обновленный счет</param>
        /// <returns>True, если обновление выполнено успешно</returns>
        bool UpdateAccount(Models.Account account);

        /// <summary>
        /// Получает счет по его идентификатору
        /// </summary>
        /// <param name="accountId">Идентификатор счета</param>
        /// <returns>Объект счета</returns>
        Models.Account GetAccount(int accountId);

        /// <summary>
        /// Получает список всех счетов пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Список счетов</returns>
        IEnumerable<Models.Account> GetUserAccounts(int userId);

        /// <summary>
        /// Получает общий баланс по всем счетам пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Сумма денежных средств на всех счетах</returns>
        decimal GetTotalBalance(int userId);

        /// <summary>
        /// Асинхронно обновляет балансы всех счетов пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Задача обновления балансов</returns>
        Task UpdateAllBalancesAsync(int userId);
    }
}