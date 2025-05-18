using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс, представляющий пользователя системы
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        private string _name;
        private string _email;
        private ObservableCollection<Account> _accounts;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public User()
        {
            _accounts = new ObservableCollection<Account>();
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        /// <summary>
        /// Счета пользователя
        /// </summary>
        public ObservableCollection<Account> Accounts => _accounts;

        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Добавляет счет пользователю
        /// </summary>
        /// <param name="account">Добавляемый счет</param>
        public void AddAccount(Account account)
        {
            if (!_accounts.Contains(account))
            {
                _accounts.Add(account);
                OnPropertyChanged(nameof(Accounts));
            }
        }

        /// <summary>
        /// Удаляет счет пользователя
        /// </summary>
        /// <param name="account">Удаляемый счет</param>
        public void RemoveAccount(Account account)
        {
            if (_accounts.Contains(account))
            {
                _accounts.Remove(account);
                OnPropertyChanged(nameof(Accounts));
            }
        }

        /// <summary>
        /// Получает общий баланс по всем счетам пользователя
        /// </summary>
        /// <returns>Сумма балансов всех счетов</returns>
        public decimal GetTotalBalance()
        {
            return _accounts.Sum(a => a.Balance);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}