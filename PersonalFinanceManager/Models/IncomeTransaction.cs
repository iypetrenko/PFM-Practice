using System;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления доходных транзакций
    /// </summary>
    public class IncomeTransaction : Transaction
    {
        private string _source;

        /// <summary>
        /// Источник дохода (например, "Работа", "Инвестиции", "Подарок")
        /// </summary>
        public string Source
        {
            get => _source;
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Валидация данных доходной транзакции
        /// </summary>
        public override void Validate()
        {
            if (Amount <= 0)
                throw new ArgumentException("Сумма дохода должна быть положительной.");

            if (string.IsNullOrWhiteSpace(Source))
                throw new ArgumentException("Источник дохода должен быть указан.");
        }

        /// <summary>
        /// Получение детальной информации о доходной транзакции
        /// </summary>
        /// <returns>Строка с информацией о доходной транзакции</returns>
        public override string GetTransactionDetails()
        {
            return $"{Date:dd.MM.yyyy} | +{Amount:C} | {Category?.Name ?? "Без категории"} | Источник: {Source} | {Description}";
        }
    }
}