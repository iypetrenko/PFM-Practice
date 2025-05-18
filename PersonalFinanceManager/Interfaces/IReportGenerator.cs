using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Interfaces
{
    /// <summary>
    /// Интерфейс для генерации финансовых отчетов
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Генерирует отчет за указанный период времени
        /// </summary>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Объект отчета</returns>
        Models.Report GenerateReport(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Генерирует отчет по конкретной категории за указанный период
        /// </summary>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Объект отчета</returns>
        Models.Report GenerateReportByCategory(int categoryId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Генерирует отчет о движении средств по конкретному счету
        /// </summary>
        /// <param name="accountId">Идентификатор счета</param>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Объект отчета</returns>
        Models.Report GenerateAccountReport(int accountId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Асинхронно генерирует сводный отчет по всем категориям и счетам
        /// </summary>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Задача, результатом которой является объект отчета</returns>
        Task<Models.Report> GenerateSummaryReportAsync(DateTime startDate, DateTime endDate);
    }
}