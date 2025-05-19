using System.Windows;
using System.Windows.Input;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Repository.Interface;

namespace PersonalFinanceManager
{
    public partial class Register
    {
        // Репозиторий пользователей для работы с данными пользователей
        private readonly IUserRepository _userRepository = new UserRepository();

        public Register()
        {
            InitializeComponent(); // Инициализация компонентов окна
        }

        // Обработчик кнопки минимизации окна
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Обработчик кнопки закрытия окна
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Обработчик перехода на окно входа
        private void Login_Clicked(object sender, RoutedEventArgs e)
        {
            NavigateToLogin();
        }

        // Обработчик кнопки регистрации
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterUser();
        }

        // Метод перехода на окно входа
        private void NavigateToLogin()
        {
            var login = new Login();
            login.Show();
            Close();
        }

        // Обработчик нажатия клавиши Enter для регистрации
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisterUser();
            }
        }

        // Метод регистрации пользователя
        private void RegisterUser()
        {
            // Проверка на заполненность полей имени пользователя и пароля
            if (string.IsNullOrEmpty(UserName.Text) || string.IsNullOrEmpty(PasswordText.Password))
            {
                MessageBox.Show(Messages.MissingInfo, "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Попытка регистрации пользователя через репозиторий
            var result = _userRepository.RegisterUser(UserName.Text, PasswordText.Password);
            if (result)
            {
                NavigateToLogin(); // При успешной регистрации переход на окно входа
            }
            else
            {
                MessageBox.Show(Messages.SomethingWrong, "Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}