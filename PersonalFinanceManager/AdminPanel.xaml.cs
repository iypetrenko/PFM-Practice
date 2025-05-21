using System.Collections.Generic;
using System.Windows;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Services;

namespace PersonalFinanceManager
{
    public partial class AdminPanel : Window
    {
        private readonly User _currentUser;
        private readonly UserRepository _userRepository = new UserRepository();

        public AdminPanel(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Loaded += AdminPanel_Loaded;
        }

        private void AdminPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (!CheckAdminAccess()) return;
            LoadUsers();
        }

        private bool CheckAdminAccess()
        {
            // Добавляем подробную проверку
            if (_currentUser == null || _currentUser.Role != UserRole.Admin)
            {
                MessageBox.Show($"Доступ запрещен. Роль пользователя: {_currentUser?.Role.ToString() ?? "null"}",
                              "Ошибка доступа",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                Close();
                return false;
            }
            return true;
        }

        private void LoadUsers()
        {
            try
            {
                usersGrid.ItemsSource = _userRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}");
            }
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnExportUserData_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAdminAccess()) return;

            var selectedUser = usersGrid.SelectedItem as User;
            if (selectedUser == null)
            {
                MessageBox.Show("Виберіть користувача!");
                return;
            }

            try
            {
                var exportService = new ExcelExportService(); // Create an instance of ExcelExportService
                exportService.ExportUserData(selectedUser);   // Use the instance to call the method
                MessageBox.Show("Експорт завершено успішно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка експорту: {ex.Message}");
            }
        }

        private void BtnViewUserData_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = usersGrid.SelectedItem as User;
            if (selectedUser == null) return;

            var userDetails = new UserDetailsWindow(selectedUser);
            userDetails.ShowDialog();
        }
    }
}