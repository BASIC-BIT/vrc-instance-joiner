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
                // Call InitializeComponent to load and connect the XAML UI
                InitializeComponent();
                
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
                var groupSelectionView = new GroupSelectionView();
                groupSelectionView.Margin = new Thickness(16);
                groupSelectionView.DataContext = _groupViewModel;
                
                // Add the GroupSelectionView to the main window
                // Look for a container in XAML, or fallback to Content
                var mainContent = Content as Panel;
                if (mainContent == null)
                {
                    // Create a container if none exists in XAML
                    var dockPanel = new DockPanel();
                    Content = dockPanel;
                    mainContent = dockPanel;
                    LogToFile("Created new DockPanel as container");
                }
                
                // Add the view to whatever container we have
                if (mainContent is Panel panel)
                {
                    panel.Children.Add(groupSelectionView);
                    LogToFile("GroupSelectionView added to panel");
                }
                
                // Initialize the GroupViewModel
                _ = _groupViewModel.InitializeAsync();
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