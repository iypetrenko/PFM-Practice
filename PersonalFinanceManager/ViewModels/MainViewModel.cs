using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceManager.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Commands;

namespace PersonalFinanceManager.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        private Transaction _selectedTransaction;
        private Category _selectedCategory;
        private decimal _currentBalance;

        /// <summary>
        /// Design-time constructor - needed for XAML designer
        /// </summary>
        public MainViewModel()
        {
            // Design-time services with minimal implementation
            _transactionService = new DesignTimeTransactionService();
            _categoryService = new DesignTimeCategoryService();

            LoadDataCommand = new Commands.RelayCommand(async () => await LoadData());
            AddTransactionCommand = new Commands.RelayCommand(AddTransaction);
            DeleteTransactionCommand = new Commands.RelayCommand(DeleteTransaction, CanDeleteTransaction);
            AddCategoryCommand = new Commands.RelayCommand(AddCategory);
        }

        /// <summary>
        /// Runtime constructor with dependency injection
        /// </summary>
        public MainViewModel(
            ITransactionService transactionService,
            ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;

            LoadDataCommand = new Commands.RelayCommand(async () => await LoadData());
            AddTransactionCommand = new Commands.RelayCommand(AddTransaction);
            DeleteTransactionCommand = new Commands.RelayCommand(DeleteTransaction, CanDeleteTransaction);
            AddCategoryCommand = new Commands.RelayCommand(AddCategory);

            // Initial load
            LoadDataCommand.Execute(null);
        }

        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set => SetProperty(ref _selectedTransaction, value);
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public decimal CurrentBalance
        {
            get => _currentBalance;
            set => SetProperty(ref _currentBalance, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }
        public ICommand AddCategoryCommand { get; }

        private async Task LoadData()
        {
            var transactions = await _transactionService.GetTransactions();
            var categories = await _categoryService.GetCategories();

            Transactions.Clear();
            Categories.Clear();

            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }

            foreach (var category in categories)
            {
                Categories.Add(category);
            }

            UpdateBalance();
        }

        private async void AddTransaction()
        {
            var newTransaction = new Transaction
            {
                Date = DateTime.Now,
                Amount = 0,
                Description = "New Transaction", // Add a default description
                CategoryId = SelectedCategory?.Id ?? 0
            };

            await _transactionService.AddTransaction(newTransaction);
            Transactions.Add(newTransaction);
            UpdateBalance();
        }

        private void DeleteTransaction()
        {
            if (SelectedTransaction != null)
            {
                _transactionService.DeleteTransaction(SelectedTransaction.Id);
                Transactions.Remove(SelectedTransaction);
                UpdateBalance();
            }
        }

        private bool CanDeleteTransaction() => SelectedTransaction != null;

        private void AddCategory()
        {
            var newCategory = new Category { Name = "New Category" };
            _categoryService.AddCategory(newCategory);
            Categories.Add(newCategory);
        }

        private void UpdateBalance()
        {
            CurrentBalance = Transactions.Sum(t => t.Amount);
        }
    }

    // Design-time service implementations - updated to match interface return types
    internal class DesignTimeTransactionService : ITransactionService
    {
        public Task<IEnumerable<Transaction>> GetTransactions()
        {
            // Return sample data for design-time  
            return Task.FromResult<IEnumerable<Transaction>>(new List<Transaction>
           {
               new Transaction { Id = 1, Date = DateTime.Now, Amount = 100, Description = "Sample Income" },
               new Transaction { Id = 2, Date = DateTime.Now, Amount = -50, Description = "Sample Expense" }
           });
        }

        public Task AddTransaction(Transaction transaction) => Task.CompletedTask;

        public Task DeleteTransaction(int id)
        {
            // Simulate async operation for design-time  
            return Task.CompletedTask;
        }
    }

    internal class DesignTimeCategoryService : ICategoryService
    {
        public Task<IEnumerable<Category>> GetCategories()
        {
            // Return sample data for design-time
            return Task.FromResult<IEnumerable<Category>>(new List<Category>
            {
                new Category { Id = 1, Name = "Income" },
                new Category { Id = 2, Name = "Expenses" }
            });
        }

        public Task AddCategory(Category category)
        {
            // Simulate async operation for design-time
            return Task.CompletedTask;
        }
    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}