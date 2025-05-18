using System;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления доходных транзакций
    /// </summary>
    // Models/IncomeTransaction.cs
    public class IncomeTransaction : Transaction
    {
        public string Source { get; set; }

        public override void Validate()
        {
            if (Amount <= 0) throw new ArgumentException("Amount must be positive");
        }

        public override string GetTransactionDetails()
        {
            return $"Income: {Amount:C} - {Source}";
        }
    }
}