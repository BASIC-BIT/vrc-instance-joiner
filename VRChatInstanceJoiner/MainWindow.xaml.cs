using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;
using VRChatInstanceJoiner.ViewModels;
using VRChatInstanceJoiner.Views;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
                
                // Initialize component from XAML
                InitializeComponent();
                
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
                // Create a simple logger for the GroupViewModel
                var logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<GroupViewModel>();
                
                // Initialize the GroupViewModel
                _groupViewModel = new GroupViewModel(_vrchatApiService, _dataStorageService, logger);
                
                // Find the GroupSelectionView in the visual tree
                var groupSelectionView = FindVisualChild<GroupSelectionView>(this);
                if (groupSelectionView != null)
                {
                    // Set the DataContext of the GroupSelectionView
                    groupSelectionView.DataContext = _groupViewModel;
                    
                    // Initialize the GroupViewModel
                    _ = _groupViewModel.InitializeAsync();
                    
                    LogToFile("GroupViewModel initialized and bound to view");
                }
                else
                {
                    LogToFile("Error: GroupSelectionView not found in visual tree");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Loaded event: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                LogToFile($"Error in Loaded event: {ex.Message}");
                LogToFile($"Stack trace: {ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// Finds a visual child of the specified type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the child to find.</typeparam>
        /// <param name="parent">The parent element to search in.</param>
        /// <returns>The first child of the specified type, or null if not found.</returns>
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T typedChild)
                {
                    return typedChild;
                }
                
                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            
            return null;
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