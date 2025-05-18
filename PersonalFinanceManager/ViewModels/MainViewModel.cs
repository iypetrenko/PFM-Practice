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

            InitializeCommands();
        }

        /// <summary>
        /// Runtime constructor with dependency injection
        /// </summary>
        public MainViewModel(
            ITransactionService transactionService,
            ICategoryService categoryService)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            InitializeCommands();

            // Load data automatically when view model is created
            LoadDataCommand.Execute(null);
        }

        private void InitializeCommands()
        {
            LoadDataCommand = new AsyncRelayCommand(LoadData);
            AddTransactionCommand = new AsyncRelayCommand(AddTransaction);
            DeleteTransactionCommand = new RelayCommand<object>(DeleteTransaction, CanDeleteTransaction);
            AddCategoryCommand = new RelayCommand(AddCategory);
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

        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddTransactionCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }
        public ICommand AddCategoryCommand { get; private set; }

        private async Task LoadData()
        {
            try
            {
                var transactions = await _transactionService.GetTransactions();
                var categories = await _categoryService.GetCategories();

                // Update UI on the UI thread
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
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
                });
            }
            catch (Exception ex)
            {
                // Log or handle exception
                System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        private async Task AddTransaction()
        {
            try
            {
                var newTransaction = new Transaction
                {
                    Date = DateTime.Now,
                    Amount = 0,
                    Description = "New Transaction",
                    CategoryId = SelectedCategory?.Id ?? 0
                };

                await _transactionService.AddTransaction(newTransaction);

                // Update UI on the UI thread
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    Transactions.Add(newTransaction);
                    UpdateBalance();
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding transaction: {ex.Message}");
            }
        }

        private void DeleteTransaction(object parameter)
        {
            try
            {
                if (SelectedTransaction != null)
                {
                    _transactionService.DeleteTransaction(SelectedTransaction.Id);
                    Transactions.Remove(SelectedTransaction);
                    UpdateBalance();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting transaction: {ex.Message}");
            }
        }

        private bool CanDeleteTransaction(object parameter) => SelectedTransaction != null;

        private void AddCategory()
        {
            try
            {
                var newCategory = new Category { Name = "New Category" };
                _categoryService.AddCategory(newCategory);
                Categories.Add(newCategory);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding category: {ex.Message}");
            }
        }

        private void UpdateBalance()
        {
            CurrentBalance = Transactions.Sum(t => t.Amount);
        }
    }

    // Design-time service implementations
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

        public Task DeleteTransaction(int id) => Task.CompletedTask;
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

        public Task AddCategory(Category category) => Task.CompletedTask;
    }
}