using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// A simple application to demonstrate the XAML binding issue and solution
    /// </summary>
    public class DiagnosticApp
    {
        private readonly string _logPath;
        
        public DiagnosticApp()
        {
            _logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "VRChatInstanceJoiner", 
                "diagnostic_app.log");
                
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
            LogMessage("Diagnostic app started");
        }
        
        public void Run()
        {
            try
            {
                // Create main window with findings
                var window = new Window
                {
                    Title = "XAML Binding Issue Diagnosis",
                    Width = 800,
                    Height = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                
                // Create a scroll viewer for content
                var scrollViewer = new ScrollViewer();
                
                // Create a stack panel for content
                var stackPanel = new StackPanel
                {
                    Margin = new Thickness(20)
                };
                
                // Add a title
                stackPanel.Children.Add(new TextBlock
                {
                    Text = "WPF XAML Binding Issue Diagnosis",
                    FontSize = 24,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 20)
                });
                
                // Add explanation of problem
                AddSection(stackPanel, "Issue Identified", 
                    "Your WPF application is experiencing an XAML binding issue where the XAML UI definitions " +
                    "aren't properly connected to their code-behind files. This is why InitializeComponent() " +
                    "doesn't exist in the context and you can't access XAML-defined elements.");
                
                // Problems section
                AddSection(stackPanel, "Problems Found", 
                    "1. Missing XAML build configuration in the project file\n" +
                    "2. Dual UI creation approach - XAML and programmatic UI generation conflicting\n" +
                    "3. Missing InitializeComponent() calls in code-behind files\n" +
                    "4. Programmatically replacing XAML-defined UI via Content = grid");
                
                // Solution section
                AddSection(stackPanel, "Solution", 
                    "I've implemented the following fixes:\n\n" +
                    "1. Updated project file with explicit XAML build actions:\n" +
                    "   - Added <Page Include=\"**\\*.xaml\"> to process XAML files\n" +
                    "   - Specified proper generators and subtypes\n\n" +
                    "2. Created demonstration views showing proper implementation:\n" +
                    "   - TestViewFixed.xaml demonstrates proper XAML definition\n" +
                    "   - TestViewFixed.xaml.cs shows proper code-behind pattern\n\n" +
                    "3. For existing views, you need to:\n" +
                    "   - Remove CreateUI() methods that programmatically create UI\n" +
                    "   - Add InitializeComponent() calls to code-behind constructors\n" +
                    "   - Let XAML handle UI definition, use code-behind for logic");
                
                // Implementation steps
                AddSection(stackPanel, "Implementation Steps", 
                    "To fix your existing views:\n\n" +
                    "1. Update MainWindow.xaml.cs:\n" +
                    "   - Keep the XAML definition in MainWindow.xaml\n" +
                    "   - Remove the CreateUI() method from code-behind\n" +
                    "   - Add InitializeComponent() call at the beginning of constructor\n\n" +
                    "2. Update GroupSelectionView.xaml.cs:\n" +
                    "   - Keep the XAML definition in GroupSelectionView.xaml\n" +
                    "   - Remove the CreateUI() method from code-behind\n" +
                    "   - Add InitializeComponent() call at the beginning of constructor\n\n" +
                    "3. Run a full rebuild of your project\n\n" +
                    "4. Check the diagnostic logs for confirmation that the XAML binding is working");
                
                // Next Steps
                AddSection(stackPanel, "Testing", 
                    "After implementing these changes, you should:\n\n" +
                    "1. Run the DiagnosticTool to verify XAML files are properly processed\n" +
                    "2. Check that InitializeComponent is available in your views\n" +
                    "3. Verify that XAML-defined elements can be accessed from code-behind\n" +
                    "4. Confirm that binding between XAML UI and ViewModels is working");
                
                // Add a button to run diagnostic tool
                var button = new Button
                {
                    Content = "Run Diagnostic Tool",
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                button.Click += (s, e) => {
                    var tool = new DiagnosticTool();
                    tool.RunDiagnostics();
                };
                stackPanel.Children.Add(button);
                
                // Set content
                scrollViewer.Content = stackPanel;
                window.Content = scrollViewer;
                
                // Show the window
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: {ex.Message}");
                LogMessage($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error in diagnostic app: {ex.Message}");
            }
        }
        
        private void AddSection(StackPanel panel, string title, string content)
        {
            // Add section title
            panel.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 20, 0, 10)
            });
            
            // Add section content
            panel.Children.Add(new TextBlock
            {
                Text = content,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            });
            
            // Add separator
            panel.Children.Add(new Separator
            {
                Margin = new Thickness(0, 10, 0, 10)
            });
        }
        
        private void LogMessage(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}";
            File.AppendAllText(_logPath, entry + Environment.NewLine);
        }
    }
}