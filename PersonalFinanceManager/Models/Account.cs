using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления финансового счета пользователя
    /// </summary>
    public class Account : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private decimal _balance;
        private string _currency;
        private string _description;
        private int _userId;
        private ObservableCollection<Transaction> _transactions;

        /// <summary>
        /// Уникальный идентификатор счета
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Название счета
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Текущий баланс счета
        /// </summary>
        public decimal Balance
        {
            get => _balance;
            private set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Валюта счета
        /// </summary>
        public string Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Описание счета
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Идентификатор пользователя-владельца счета
        /// </summary>
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция транзакций, связанных с этим счетом
        /// </summary>
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions ??= new ObservableCollection<Transaction>();
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Событие, происходящее при добавлении новой транзакции в счет
        /// </summary>
        public event EventHandler<Transaction> OnTransactionAdded;

        /// <summary>
        /// Добавляет новую транзакцию в счет и обновляет баланс
        /// </summary>
        /// <param name="transaction">Транзакция для добавления</param>
        public void AddTransaction(Transaction transaction)
        {
            transaction.Validate();
            transaction.AccountId = Id;

            Transactions.Add(transaction);
            UpdateBalance();

            OnTransactionAdded?.Invoke(this, transaction);
        }

        /// <summary>
        /// Обновляет баланс счета на основе всех транзакций
        /// </summary>
        public void UpdateBalance()
        {
            Balance = Transactions.Sum(t => t.Amount);
        }

        /// <summary>
        /// Возвращает историю транзакций счета за указанный период
        /// </summary>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Список транзакций за указанный период</returns>
        public IEnumerable<Transaction> GetTransactionHistory(DateTime startDate, DateTime endDate)
        {
            return Transactions.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
        }

        /// <summary>
        /// Возвращает информацию о счете в виде строки
        /// </summary>
        /// <returns>Строка с информацией о счете</returns>
        public string GetAccountInfo()
        {
            return $"{Name} | Баланс: {Balance:C} {Currency} | {Description}";
        }

        // Реализация интерфейса INotifyPropertyChanged для поддержки привязки данных в MVVM
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}