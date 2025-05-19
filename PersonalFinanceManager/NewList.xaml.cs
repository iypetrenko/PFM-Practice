using System.Windows;
using System.Windows.Input;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager
{
    public partial class NewList
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository = new ExpenseCategoryRepository();

        public NewList()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewList();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewList();
            }
        }

        private void AddNewList()
        {
            if (string.IsNullOrEmpty(NewCategoryName.Text))
            {
                MessageBox.Show(Messages.MissingInfo, "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }

            if (string.IsNullOrEmpty(NewCategoryBudget.Text))
            {
                MessageBox.Show(Messages.MissingBudget, "Failed",   
                    MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
                return;
            }

            var budgetValue = decimal.Parse(NewCategoryBudget.Text);
            var result = _expenseCategoryRepository.AddExpenseCategory(NewCategoryName.Text, budgetValue);
            DialogResult = result;
        }
    }
}