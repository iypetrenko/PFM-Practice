using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using System.Collections.Generic;
using System.Windows.Input;

namespace PersonalFinanceManager.ViewModels
{
    public class AddEditTransactionViewModel : BaseViewModel
    {
        private readonly TransactionService _transactionService;
        public Transaction Transaction { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> TransactionTypes => new List<string> { "Income", "Expense" };

        public ICommand SaveCommand { get; }

        // Replace the abstract Transaction with a concrete implementation
        public class ConcreteTransaction : Transaction
        {
            public override void Validate()
            {
                // Add validation logic here
            }

            public override string GetTransactionDetails()
            {
                return $"Transaction Details: Amount={Amount}, Date={Date}, CategoryId={CategoryId}, AccountId={AccountId}";
            }
        }

        // Update the ViewModel to use the concrete implementation
        public AddEditTransactionViewModel(TransactionService transactionService, CategoryService categoryService)
        {
            _transactionService = transactionService;
            Categories = categoryService.GetCategories();
            SaveCommand = new RelayCommand(Save);
            Transaction = new ConcreteTransaction { Date = DateTime.Now }; // Use the concrete class
        }


        private void Save()
        {
            _transactionService.AddTransaction(Transaction);
            OnRequestClose?.Invoke();
        }

        // Для закрытия окна после сохранения
        public event Action OnRequestClose;
    }
}