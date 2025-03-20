using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VRChatInstanceJoiner.Services;

namespace VRChatInstanceJoiner;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider serviceProvider;
    private string logFilePath;

    public App()
    {
        // Set up logging
        logFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VRChatInstanceJoiner", 
            "app_log.txt");
            
        // Ensure directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
        
        // Log application start
        LogToFile("Application starting...");
        
        // Set up unhandled exception handlers
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        
        try
        {
            // Set up dependency injection
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            LogToFile("Services configured successfully");
        }
        catch (Exception ex)
        {
            LogToFile($"Error during initialization: {ex.Message}");
            LogToFile($"Stack trace: {ex.StackTrace}");
            MessageBox.Show($"Error during initialization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Add logging
        services.AddLogging();

        // Register services
        services.AddSingleton<IDataStorageService, DataStorageService>();
        services.AddSingleton<IVRChatApiService, VRChatApiService>();

        // Register main window
        services.AddTransient<MainWindow>();
        
        LogToFile("Services registered");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            LogToFile("OnStartup called");
            base.OnStartup(e);

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            LogToFile("MainWindow created");
            
            mainWindow.Show();
            LogToFile("MainWindow.Show() called");
        }
        catch (Exception ex)
        {
            LogToFile($"Error during startup: {ex.Message}");
            LogToFile($"Stack trace: {ex.StackTrace}");
            MessageBox.Show($"Error during startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        LogToFile("Application exiting");
        serviceProvider.Dispose();
        base.OnExit(e);
    }
    
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        LogToFile($"Unhandled exception: {ex?.Message}");
        LogToFile($"Stack trace: {ex?.StackTrace}");
        MessageBox.Show($"An unhandled exception occurred: {ex?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        LogToFile($"Dispatcher unhandled exception: {e.Exception.Message}");
        LogToFile($"Stack trace: {e.Exception.StackTrace}");
        MessageBox.Show($"An unhandled exception occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
    
    private void LogToFile(string message)
    {
        try
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}");
        }
        catch
        {
            // Ignore errors during logging
        }
    }
}
