using System;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления расходных транзакций
    /// </summary>
    // Models/ExpenseTransaction.cs
    public class ExpenseTransaction : Transaction
    {
        public string Description { get; set; }

        public override void Validate()
        {
            if (Amount <= 0) throw new ArgumentException("Amount must be positive");
        }

        public override string GetTransactionDetails()
        {
            return $"Expense: {Amount:C} - {Description}";
        }
    }
}