using System.Windows;
using System.Windows.Input;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager
{
    public partial class Login
    {
        private readonly IUserRepository _userRepository = new UserRepository();

        public Login()
        {
            InitializeComponent();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Register_Clicked(object sender, RoutedEventArgs e)
        {
            var register = new Register();
            register.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginUser();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginUser();
            }
        }

        private void LoginUser()
        {
            if (string.IsNullOrEmpty(UserName.Text) || string.IsNullOrEmpty(PasswordText.Password))
            {
                MessageBox.Show(Messages.MissingInfo, "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Специальная обработка для администратора
            if (UserName.Text.ToLower() == "admin" && PasswordText.Password == "admin")
            {
                _userRepository.CreateAdminIfNotExists();
                var admin = _userRepository.CheckLogin("admin", "admin");

                if (admin != null)
                {
                    SessionInfo.UserId = admin.Id;
                    var mainPage = new MainWindow(admin);
                    mainPage.Show();
                    Close();
                    return;
                }
            }

            var result = _userRepository.CheckLogin(UserName.Text, PasswordText.Password);
            if (result != null)
            {
                SessionInfo.UserId = result.Id;
                var mainPage = new MainWindow(result);
                mainPage.Show();
                Close();
            }
            else
            {
                MessageBox.Show(Messages.WrongLogin, "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ClearFields();
            }
        }
        private void BtnGuestLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var guestUser = _userRepository.CreateGuestUser();
                SessionInfo.UserId = guestUser.Id; // Важно обновить сессию
                var mainWindow = new MainWindow(guestUser);
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания гостевого пользователя: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ClearFields()
        {
            UserName.Text = "";
            PasswordText.Password = "";
        }
    }
}