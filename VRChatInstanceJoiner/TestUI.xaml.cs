using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Interaction logic for TestUI.xaml
    /// </summary>
    public partial class TestUI : UserControl
    {
        public TestUI()
        {
            // Diagnostic logging
            string logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "VRChatInstanceJoiner", 
                "xaml_diagnostic.log");
            
            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                File.AppendAllText(logPath, $"{DateTime.Now}: TestUI constructor called\n");
                
                // Try to find generated files
                var objDir = Path.Combine(Directory.GetCurrentDirectory(), "obj");
                if (Directory.Exists(objDir))
                {
                    var generatedFiles = Directory.GetFiles(objDir, "*.g.cs", SearchOption.AllDirectories);
                    File.AppendAllText(logPath, $"Found {generatedFiles.Length} generated files:\n");
                    foreach (var file in generatedFiles)
                    {
                        File.AppendAllText(logPath, $"  {file}\n");
                    }
                }
                
                // Log build actions for XAML files
                File.AppendAllText(logPath, "Attempting to call InitializeComponent()...\n");
                
                try 
                {
                    // This will fail if the XAML is not being processed correctly
                    InitializeComponent();
                    File.AppendAllText(logPath, "InitializeComponent succeeded\n");
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logPath, $"InitializeComponent FAILED: {ex.Message}\n");
                    File.AppendAllText(logPath, $"Stack trace: {ex.StackTrace}\n");
                    
                    // Display error in UI since InitializeComponent failed
                    var grid = new Grid();
                    var text = new TextBlock
                    {
                        Text = "InitializeComponent failed! The XAML file is not being correctly processed during build.",
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(20)
                    };
                    grid.Children.Add(text);
                    Content = grid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in TestUI: {ex.Message}");
            }
        }
    }
}