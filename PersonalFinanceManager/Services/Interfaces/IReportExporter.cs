using System;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для экспорта финансовых отчетов в различные форматы
    /// </summary>
    public interface IReportExporter
    {
        /// <summary>
        /// Экспортирует отчет в формат CSV
        /// </summary>
        /// <param name="report">Отчет для экспорта</param>
        /// <param name="filePath">Путь для сохранения файла</param>
        /// <returns>True, если экспорт выполнен успешно</returns>
        bool ExportToCSV(Models.Report report, string filePath);

        /// <summary>
        /// Экспортирует отчет в формат PDF
        /// </summary>
        /// <param name="report">Отчет для экспорта</param>
        /// <param name="filePath">Путь для сохранения файла</param>
        /// <returns>True, если экспорт выполнен успешно</returns>
        bool ExportToPDF(Models.Report report, string filePath);

        /// <summary>
        /// Экспортирует отчет в формат Excel
        /// </summary>
        /// <param name="report">Отчет для экспорта</param>
        /// <param name="filePath">Путь для сохранения файла</param>
        /// <returns>True, если экспорт выполнен успешно</returns>
        bool ExportToExcel(Models.Report report, string filePath);

        /// <summary>
        /// Асинхронно экспортирует отчет в формат JSON
        /// </summary>
        /// <param name="report">Отчет для экспорта</param>
        /// <param name="filePath">Путь для сохранения файла</param>
        /// <returns>Задача, результатом которой является успешность экспорта</returns>
        Task<bool> ExportToJsonAsync(Models.Report report, string filePath);
    }
}