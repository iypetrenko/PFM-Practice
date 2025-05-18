using System;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления расходных транзакций
    /// </summary>
    public class ExpenseTransaction : Transaction
    {
        private string _recipient;

        /// <summary>
        /// Получатель платежа (например, "Магазин X", "Коммунальные услуги")
        /// </summary>
        public string Recipient
        {
            get => _recipient;
            set
            {
                _recipient = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Валидация данных расходной транзакции
        /// </summary>
        public override void Validate()
        {
            if (Amount >= 0)
                throw new ArgumentException("Сумма расхода должна быть отрицательной.");

            if (string.IsNullOrWhiteSpace(Recipient))
                throw new ArgumentException("Получатель платежа должен быть указан.");
        }

        /// <summary>
        /// Получение детальной информации о расходной транзакции
        /// </summary>
        /// <returns>Строка с информацией о расходной транзакции</returns>
        public override string GetTransactionDetails()
        {
            return $"{Date:dd.MM.yyyy} | {Amount:C} | {Category?.Name ?? "Без категории"} | Получатель: {Recipient} | {Description}";
        }
    }
}