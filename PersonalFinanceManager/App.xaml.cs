using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.ViewModels;
using PersonalFinanceManager.Views; // Ensure this namespace is correct and the file exists

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        // Ensure MainWindow is instantiated with required dependencies
        var transactionService = ServiceProvider.GetRequiredService<ITransactionService>();
        var categoryService = ServiceProvider.GetRequiredService<ICategoryService>();
        var mainWindow = new MainWindow(transactionService, categoryService);
        mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
        mainWindow.Show();
    }


    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITransactionService, TransactionService>();
        services.AddSingleton<ICategoryService, CategoryService>();
        services.AddSingleton<MainViewModel>();
    }
}
