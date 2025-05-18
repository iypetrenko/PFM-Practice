using PersonalFinanceManager.Services;
using System.Windows.Input;

namespace PersonalFinanceManager.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(Login);
            NavigateToRegisterCommand = new RelayCommand(() => { /* Навигация на регистрацию */ });
        }

        private void Login()
        {
            bool isAuthenticated = _authService.Login(Username, Password);
            if (isAuthenticated)
            {
                // Переход на главный экран
            }
        }
    }
}