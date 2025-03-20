using System;
using System.Windows;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataStorageService _dataStorageService;
        private AppSettings? _settings;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize services
            _dataStorageService = new DataStorageService(null);
            
            // Load settings
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                _settings = _dataStorageService.LoadSettingsAsync().GetAwaiter().GetResult();
                Console.WriteLine("Settings loaded successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
                _settings = new AppSettings(); // Use defaults
            }
        }
    }
}