using PersonalFinanceManager.Migrations;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using System.Data.Entity;
using System.Windows;

namespace PersonalFinanceManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация администратора
            var userRepository = new UserRepository();
            userRepository.CreateAdminIfNotExists();

            // Проверка и применение миграций
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PersonalFinanceManagerContext, Configuration>());
        }
    }
}