using System.Windows;
using System.Windows.Controls;
using PersonalFinanceManager;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using User = PersonalFinanceManager.Model.User;
namespace PersonalFinanceManager
{
    public partial class MainWindow
    {
        private readonly IExpenseCategoryRepository _toDoListRepository = new ExpenseCategoryRepository();
        private User _currentUser;
        public MainWindow(User user)
        {   
            InitializeComponent();
            _currentUser = user;
            Console.WriteLine("Jira integration works!");

            ConfigureUIForUserRole();
            LoadToDoLists();
        }
        private void ConfigureUIForUserRole()
        {
            if (_currentUser == null) return;

            // Скрываем админ-панель для всех кроме админов
            btnAdminPanel.Visibility = _currentUser.Role == UserRole.Admin
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Скрываем бюджет только для гостей
            btnBudgetStatus.Visibility = _currentUser.Role == UserRole.Guest
                ? Visibility.Collapsed
                : Visibility.Visible;

            // Всегда показываем конвертер
            btnCurrencyConverter.Visibility = Visibility.Visible;
        }

        private void SeeConverter_Click(object sender, RoutedEventArgs e)
        {
            var converter = new ConverterWindow(_currentUser);
            converter.Show();
            Close();
        }

        private void BtnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel(_currentUser);
            adminPanel.Show();
        }

        private void LoadToDoLists()
        {
            RemoveStackPanelItems();
            var result = _toDoListRepository.GetExpenseCategoriesList();
            foreach (var item in result)
            {
                var newBtn = new Button
                {
                    Content = item.Name,
                    Name = "_" + item.Id
                };
                var margin = newBtn.Margin;
                margin.Top = 5;
                newBtn.Margin = margin;
                newBtn.Click += ToDoButton_Click;
                StackPanel.Children.Add(newBtn);
            }
        }

        private void RemoveStackPanelItems()
        {
            while (StackPanel.Children.Count > 0)
            {
                StackPanel.Children.RemoveAt(StackPanel.Children.Count - 1);
            }
        }



        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            var login = new Login();
            login.Show();
            Close();
        }




        private void SeeBudgetStatus_Click(object sender, RoutedEventArgs e)
        {
            var budget = new BudgetWindow(_currentUser);
            budget.Show();
            Close();
        }

        private void NewListItem_Click(object sender, RoutedEventArgs e)
        {
            var newList = new NewList();
            if (newList.ShowDialog() != true) return;
            MessageBox.Show(Messages.NewListAdded, "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
            LoadToDoLists();
        }



        private void ToDoButton_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).Name.TrimStart('_'));
            var text = ((Button)sender).Content;
            var toDoDetails = new ToDoDetails(id, text.ToString(), _currentUser);
            toDoDetails.Show();
            Close();
        }
    }
}
