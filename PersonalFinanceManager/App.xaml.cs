using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.ViewModels;
using PersonalFinanceManager.Views;

namespace PersonalFinanceManager
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            try
            {
                // Create and show the main window
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.Message}\n\n{ex.StackTrace}",
                    "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<ICategoryService, CategoryService>();

            // Register view models
            services.AddSingleton<MainViewModel>();

            // Register views
            services.AddSingleton<MainWindow>();
        }
    }
}