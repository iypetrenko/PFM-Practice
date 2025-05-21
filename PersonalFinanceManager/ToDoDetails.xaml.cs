using System;
using System.Windows;
using System.Windows.Controls;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager
{
    public partial class ToDoDetails:Window
    {
        // Репозиторий для категорий расходов
        private readonly IExpenseCategoryRepository _categoriesRepository = new ExpenseCategoryRepository();
        // Репозиторий для элементов списка
        private readonly IItemRepository _itemRepository = new ItemRepository();
        // Идентификатор текущего списка дел
        private readonly int _id;
        private User _currentUser;
        // Конструктор окна деталей списка дел
        public ToDoDetails(int id, string text, User user)
        {
            InitializeComponent();
            _currentUser = user;
            _id = id;
            TodoName.Content = text;
            LoadToDoItems();
            DatePicker.Text = DateTime.Now.ToShortDateString();
        }

        // Загрузка элементов списка дел
        public void LoadToDoItems()
        {
            var result = _itemRepository.GetItems(_id);
            expensesItemsListView.ItemsSource = result;
        }

        // Переход на главную страницу
        private void NavigateToMainPage()
        {
            var mainWindow = new MainWindow(_currentUser);
            mainWindow.Show();
            Close();
        }

        // Обработчик кнопки удаления списка
        private void ListRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(Messages.ListRemove, "Remove List",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            _categoriesRepository.RemoveExpenseCategory(_id);
            NavigateToMainPage();
        }

        // Обработчик кнопки "Назад"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToMainPage();
        }

        // Обработчик кнопки добавления нового элемента
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            var newListItem = new NewListItem(_id);
            if (newListItem.ShowDialog() != true) return;
            LoadToDoItems();
        }

        // Обработчик кнопки удаления выбранного элемента
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = expensesItemsListView.SelectedItem;
            if (selectedItem != null)
            {
                var selectedToDoItem = (Item)selectedItem;
                var result = _itemRepository.DeleteItem(selectedToDoItem);
                if (result)
                {
                    MessageBox.Show(Messages.ItemRemoved, "Success",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    LoadToDoItems();
                }
                else
                {
                    MessageBox.Show(Messages.SomethingWrong, "Fail",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(Messages.ChooseItem, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик кнопки поиска элементов
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Parse(DatePicker.Text);
            var result = _itemRepository.FilterItems(SearchText.Text, Status.Text, date);
            expensesItemsListView.ItemsSource = result;
        }

        // Обработчик кнопки очистки фильтров поиска
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Text = "";
            Status.SelectedIndex = 0;
            LoadToDoItems();
        }
    }
}