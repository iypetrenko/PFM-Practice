using System.Windows;
using System.Windows.Controls;
using PersonalFinanceManager;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager
{
    public partial class MainWindow
    {
        private readonly IExpenseCategoryRepository _toDoListRepository = new ExpenseCategoryRepository();

        public MainWindow()
        {
            InitializeComponent();
            LoadToDoLists();
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
            var budget = new BudgetWindow();
            budget.Show();
            Close();
        }

        private void SeeConverter_Click(object sender, RoutedEventArgs e)
        {
            var converter = new ConverterWindow();
            converter.Show();
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
            var toDoDetails = new ToDoDetails(id, text.ToString());
            toDoDetails.Show();
            Close();
        }
    }
}