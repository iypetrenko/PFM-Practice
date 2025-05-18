using System.Windows;

namespace PersonalFinanceManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация базы данных
            using var context = new Data.ApplicationDbContext();
            context.Database.EnsureCreated();
        }
    }
}