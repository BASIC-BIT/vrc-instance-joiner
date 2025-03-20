using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// A basic window implementation that creates UI programmatically
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataStorageService _dataStorageService;
        private readonly IVRChatApiService _vrchatApiService;
        private readonly string _logFilePath;

        public MainWindow(IDataStorageService dataStorageService, IVRChatApiService vrchatApiService)
        {
            try
            {
                // Set up logging
                _logFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "VRChatInstanceJoiner", 
                    "window_log.txt");
                
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
                
                LogToFile("MainWindow constructor called");
                
                // Initialize services
                _dataStorageService = dataStorageService;
                _vrchatApiService = vrchatApiService;
                
                LogToFile("Services initialized");
                
                // Set window properties
                Title = "VRChat Instance Joiner";
                Width = 800;
                Height = 600;
                
                // Create UI programmatically
                CreateUI();
                
                LogToFile("UI created");
                
                // Set up event handlers
                Loaded += MainWindow_Loaded;
                LogToFile("Event handlers set up");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing MainWindow: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LogToFile($"Error in constructor: {ex.Message}");
                LogToFile($"Stack trace: {ex.StackTrace}");
            }
        }

        private void CreateUI()
        {
            try
            {
                LogToFile("Creating UI");
                
                // Create main grid
                var grid = new Grid();
                Content = grid;
                
                // Create a stack panel for content
                var stackPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                
                grid.Children.Add(stackPanel);
                
                // Add title
                var titleTextBlock = new TextBlock
                {
                    Text = "VRChat Instance Joiner",
                    FontSize = 32,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 20)
                };
                
                stackPanel.Children.Add(titleTextBlock);
                
                // Add status message
                var statusTextBlock = new TextBlock
                {
                    Text = "Application is running successfully!",
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 40)
                };
                
                stackPanel.Children.Add(statusTextBlock);
                
                // Add status items
                AddStatusItem(stackPanel, "Dependency Injection: Working");
                AddStatusItem(stackPanel, "Services: Initialized");
                AddStatusItem(stackPanel, "UI: Rendered");
                
                LogToFile("UI elements created");
            }
            catch (Exception ex)
            {
                LogToFile($"Error creating UI: {ex.Message}");
                LogToFile($"Stack trace: {ex.StackTrace}");
            }
        }
        
        private void AddStatusItem(StackPanel parent, string text)
        {
            var textBlock = new TextBlock
            {
                Text = "• " + text,
                FontSize = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 5, 0, 5)
            };
            
            parent.Children.Add(textBlock);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LogToFile("MainWindow_Loaded event fired");
                
                // Display a welcome message
                MessageBox.Show("VRChat Instance Joiner is now running!", "Application Started", MessageBoxButton.OK, MessageBoxImage.Information);
                LogToFile("Welcome message displayed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Loaded event: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LogToFile($"Error in Loaded event: {ex.Message}");
                LogToFile($"Stack trace: {ex.StackTrace}");
            }
        }
        
        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}");
            }
            catch
            {
                // Ignore errors during logging
            }
        }
    }
}