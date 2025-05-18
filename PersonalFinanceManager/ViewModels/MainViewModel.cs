using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using System.Collections.ObjectModel;

namespace PersonalFinanceManager.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly TransactionService _transactionService;
        private readonly BudgetService _budgetService;

        [ObservableProperty]
        private ObservableCollection<Transaction> _transactions;

        [ObservableProperty]
        private ObservableCollection<Budget> _budgets;

        public IRelayCommand AddTransactionCommand { get; }

        public MainViewModel()
        {
            _transactionService = new TransactionService();
            _budgetService = new BudgetService();

            LoadData();

            AddTransactionCommand = new RelayCommand(AddTransaction);
        }

        private void LoadData()
        {
            Transactions = new ObservableCollection<Transaction>(
                _transactionService.GetTransactions());

            Budgets = new ObservableCollection<Budget>(
                _budgetService.GetBudgets());
        }

        private void AddTransaction()
        {
            // Логика добавления транзакции
        }
    }
}