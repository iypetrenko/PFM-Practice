using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceManager.ViewModels
{
    public class AddEditTransactionViewModel : ViewModelBase
    {
        private readonly TransactionService _transactionService;
        private readonly CategoryService _categoryService;

        public Transaction Transaction { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> TransactionTypes => new List<string> { "Income", "Expense" };

        public ICommand SaveCommand { get; }

        public AddEditTransactionViewModel(
            TransactionService transactionService,
            CategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;

            LoadCategories();
            SaveCommand = new RelayCommand(Save);
            Transaction = new Transaction { Date = DateTime.Now };
        }

        private async void LoadCategories()
        {
            Categories = (List<Category>)await _categoryService.GetCategories();
            OnPropertyChanged(nameof(Categories));
        }

        private void Save()
        {
            _transactionService.AddTransaction(Transaction);
            OnRequestClose?.Invoke();
        }

        public event Action OnRequestClose;
    }
}