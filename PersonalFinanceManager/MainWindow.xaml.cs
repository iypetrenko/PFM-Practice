
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.ViewModels;
using System.Windows;

namespace PersonalFinanceManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(ITransactionService transactionService, ICategoryService categoryService)
        {
            InitializeComponent();
            DataContext = new MainViewModel(transactionService, categoryService);
        }
    }
}