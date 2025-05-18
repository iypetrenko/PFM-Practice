
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для генерации финансовых отчетов
    /// </summary>
    public class Report : INotifyPropertyChanged
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private List<Transaction> _transactions;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Report()
        {
            _transactions = new List<Transaction>();
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Дата начала периода отчета
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        /// <summary>
        /// Дата окончания периода отчета
        /// </summary>
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        /// <summary>
        /// Транзакции, включенные в отчет
        /// </summary>
        public List<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                if (_transactions != value)
                {
                    _transactions = value;
                    OnPropertyChanged(nameof(Transactions));
                }
            }
        }

        /// <summary>
        /// Уникальный идентификатор отчета
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Название отчета
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Общая сумма доходов в отчете
        /// </summary>
        public decimal TotalIncome => Transactions
            .Where(t => t.Amount > 0)
            .Sum(t => t.Amount);

        /// <summary>
        /// Общая сумма расходов в отчете
        /// </summary>
        public decimal TotalExpense => Transactions
            .Where(t => t.Amount < 0)
            .Sum(t => t.Amount);

        /// <summary>
        /// Баланс (доходы + расходы)
        /// </summary>
        public decimal Balance => TotalIncome + TotalExpense;

        /// <summary>
        /// Генерирует отчет на основе переданных транзакций и периода
        /// </summary>
        /// <param name="transactions">Список всех транзакций</param>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Сгенерированный отчет</returns>
        public Report GenerateReport(IEnumerable<Transaction> transactions, DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            Transactions = transactions
                .Where(t => t.Date >= startDate && t.Date <= endDate)
                .ToList();

            Name = $"Отчет за период {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";

            return this;
        }

        /// <summary>
        /// Группирует транзакции по категориям
        /// </summary>
        /// <returns>Словарь, где ключ - категория, значение - сумма по этой категории</returns>
        public Dictionary<Category, decimal> GetTransactionsByCategory()
        {
            return Transactions
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        /// <summary>
        /// Группирует транзакции по датам
        /// </summary>
        /// <returns>Словарь, где ключ - дата, значение - сумма транзакций за этот день</returns>
        public Dictionary<DateTime, decimal> GetTransactionsByDate()
        {
            return Transactions
                .GroupBy(t => t.Date.Date)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}