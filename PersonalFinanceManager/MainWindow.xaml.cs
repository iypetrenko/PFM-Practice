
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.ViewModels;
using System.Windows;

namespace PersonalFinanceManager.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Уберите установку DataContext здесь.
            // DataContext будет установлен через DI в App.xaml.cs.
        }
    }
}