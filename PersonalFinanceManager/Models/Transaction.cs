using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Абстрактный класс для всех финансовых транзакций
    /// </summary>
    // Models/Transaction.cs
    public abstract class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }

        public abstract void Validate();
        public abstract string GetTransactionDetails();
    }
}