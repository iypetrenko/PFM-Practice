using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalFinanceManager.Models
{
    /// <summary>
    /// Класс для представления категорий доходов и расходов
    /// </summary>
    public class Category : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _type; // "Income" или "Expense"
        private string _description;
        private string _iconPath; // Путь к иконке категории
        private int _userId; // Идентификатор пользователя, создавшего категорию

        /// <summary>
        /// Уникальный идентификатор категории
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
        /// Название категории
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
        /// Тип категории: "Income" (доход) или "Expense" (расход)
        /// </summary>
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Описание категории
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
        /// Путь к иконке категории
        /// </summary>
        public string IconPath
        {
            get => _iconPath;
            set
            {
                _iconPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Идентификатор пользователя, создавшего категорию
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
        /// Получение информации о категории
        /// </summary>
        /// <returns>Строка с информацией о категории</returns>
        public string GetCategoryInfo()
        {
            return $"{Name} ({Type}) - {Description}";
        }

        /// <summary>
        /// Проверяет, является ли категория категорией доходов
        /// </summary>
        /// <returns>True, если категория относится к доходам</returns>
        public bool IsIncome()
        {
            return Type.Equals("Income", System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Проверяет, является ли категория категорией расходов
        /// </summary>
        /// <returns>True, если категория относится к расходам</returns>
        public bool IsExpense()
        {
            return Type.Equals("Expense", System.StringComparison.OrdinalIgnoreCase);
        }

        // Реализация интерфейса INotifyPropertyChanged для поддержки привязки данных в MVVM
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}