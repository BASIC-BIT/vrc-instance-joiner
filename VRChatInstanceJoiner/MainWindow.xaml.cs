using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;
using VRChatInstanceJoiner.ViewModels;
using VRChatInstanceJoiner.Views;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Main window for the VRChat Instance Joiner application.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataStorageService _dataStorageService;
        private readonly IVRChatApiService _vrchatApiService;
        private readonly string _logFilePath;
        private GroupViewModel _groupViewModel;

        public bool IsAuthenticated => _vrchatApiService.IsAuthenticated;

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
                
                // Create UI programmatically
                CreateUI();
                
                // Set DataContext for binding
                DataContext = this;
                
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
            // Set window properties
            Title = "VRChat Instance Joiner";
            Width = 900;
            Height = 600;
            
            // Set Material Design styles
            System.Windows.Documents.TextElement.SetForeground(this, (Brush)Application.Current.Resources["MaterialDesignBody"]);
            System.Windows.Documents.TextElement.SetFontWeight(this, FontWeights.Regular);
            System.Windows.Documents.TextElement.SetFontSize(this, 13);
            // Skip TextOptions settings as they're not critical
            // and are causing reference issues
            Background = (Brush)Application.Current.Resources["MaterialDesignPaper"];
            FontFamily = (FontFamily)Application.Current.Resources["MaterialDesignFont"];
            
            // Create a DockPanel as the main container
            var dockPanel = new DockPanel();
            Content = dockPanel;
            
            
            LogToFile("UI components created programmatically");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LogToFile("MainWindow_Loaded event fired");
                
                // Create a logger for the GroupViewModel
                var logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<GroupViewModel>();
                
                // Initialize the GroupViewModel
                _groupViewModel = new GroupViewModel(_vrchatApiService, _dataStorageService, logger);
                
                // Create a GroupSelectionView
                var groupSelectionView = new GroupSelectionView
                {
                    Margin = new Thickness(16),
                    DataContext = _groupViewModel
                };
                
                // Add the GroupSelectionView to the DockPanel
                if (Content is DockPanel dockPanel)
                {
                    dockPanel.Children.Add(groupSelectionView);
                    LogToFile("GroupSelectionView added to DockPanel");
                
                // Initialize the GroupViewModel
                _ = _groupViewModel.InitializeAsync();
                }
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