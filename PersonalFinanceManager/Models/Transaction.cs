using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Абстрактный класс для всех финансовых транзакций
    /// </summary>
    public abstract class Transaction : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _date;
        private decimal _amount;
        private string _description;
        private Category _category;
        private int _accountId;

        /// <summary>
        /// Уникальный идентификатор транзакции
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
        /// Дата проведения транзакции
        /// </summary>
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal Amount
        {
            get => _amount;
            protected set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Описание транзакции
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
        /// Категория транзакции
        /// </summary>
        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Идентификатор счета, к которому относится транзакция
        /// </summary>
        public int AccountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Абстрактный метод для валидации данных транзакции
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// Получение детальной информации о транзакции
        /// </summary>
        /// <returns>Строка с информацией о транзакции</returns>
        public virtual string GetTransactionDetails()
        {
            return $"{Date:dd.MM.yyyy} | {Amount:C} | {Category?.Name ?? "Без категории"} | {Description}";
        }

        // Реализация интерфейса INotifyPropertyChanged для поддержки привязки данных в MVVM
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}