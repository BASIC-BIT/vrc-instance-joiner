using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Diagnostic tool to identify WPF XAML compilation issues
    /// </summary>
    public class DiagnosticTool
    {
        private readonly string _logPath;
        
        public DiagnosticTool()
        {
            _logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "VRChatInstanceJoiner", 
                "xaml_diagnosis.log");
                
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
            LogMessage("Diagnostic tool started");
        }
        
        public void RunDiagnostics()
        {
            try
            {
                LogMessage("=== WPF XAML Binding Diagnostics ===");
                
                // Check project file
                CheckProjectFile();
                
                // Check temp/obj directory for generated files
                CheckGeneratedFiles();
                
                // Check assembly for expected types
                CheckAssemblyTypes();
                
                // Report environment
                ReportEnvironment();
                
                LogMessage("Diagnostics complete. Check the log at: " + _logPath);
                MessageBox.Show("Diagnostics complete. Check the log at: " + _logPath);
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: {ex.Message}");
                LogMessage($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error running diagnostics: {ex.Message}");
            }
        }
        
        private void CheckProjectFile()
        {
            LogMessage("Checking project file...");
            string projectPath = Path.Combine(Directory.GetCurrentDirectory(), "VRChatInstanceJoiner.csproj");
            
            if (File.Exists(projectPath))
            {
                string content = File.ReadAllText(projectPath);
                LogMessage($"Project file found: {projectPath}");
                
                bool hasUseWpf = content.Contains("<UseWPF>true</UseWPF>");
                LogMessage($"- Has UseWPF=true: {hasUseWpf}");
                
                bool hasPage = content.Contains("<Page ");
                LogMessage($"- Has explicit Page items: {hasPage}");
                
                // Look for build action problems
                if (!hasPage)
                {
                    LogMessage("ISSUE: No explicit Page items in project file.");
                    LogMessage("This may mean XAML files aren't being processed as WPF pages.");
                    LogMessage("SOLUTION: Add ItemGroup with Page Include for XAML files");
                }
            }
            else
            {
                LogMessage("ERROR: Project file not found");
            }
        }
        
        private void CheckGeneratedFiles()
        {
            LogMessage("Checking for generated XAML code files...");
            
            // Check obj directory for .g.cs files
            string objDir = Path.Combine(Directory.GetCurrentDirectory(), "obj");
            if (Directory.Exists(objDir))
            {
                var generatedFiles = Directory.GetFiles(objDir, "*.g.cs", SearchOption.AllDirectories);
                LogMessage($"Found {generatedFiles.Length} generated files.");
                
                if (generatedFiles.Length > 0)
                {
                    // List a few samples
                    int samplesToShow = Math.Min(5, generatedFiles.Length);
                    LogMessage("Sample generated files:");
                    for (int i = 0; i < samplesToShow; i++)
                    {
                        LogMessage($"- {Path.GetFileName(generatedFiles[i])}");
                    }
                }
                else
                {
                    LogMessage("ISSUE: No .g.cs files found!");
                    LogMessage("This means XAML files aren't being compiled properly.");
                    LogMessage("SOLUTION: Check build actions - XAML files should be 'Page'");
                }
            }
            else
            {
                LogMessage("ERROR: obj directory not found");
            }
        }
        
        private void CheckAssemblyTypes()
        {
            LogMessage("Checking assembly for expected types...");
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            LogMessage($"Current assembly: {assembly.FullName}");
            
            var types = assembly.GetTypes();
            var xamlTypes = types.Where(t => 
                typeof(FrameworkElement).IsAssignableFrom(t) && 
                t.Name != "App" &&
                !t.Name.StartsWith("<")
            ).ToList();
            
            LogMessage($"Found {xamlTypes.Count} XAML-related types:");
            foreach (var type in xamlTypes)
            {
                bool hasInitializeComponent = type.GetMethod("InitializeComponent", 
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) != null;
                    
                LogMessage($"- {type.FullName}");
                LogMessage($"  Has InitializeComponent method: {hasInitializeComponent}");
                
                if (!hasInitializeComponent)
                {
                    LogMessage("  ISSUE: Missing InitializeComponent method!");
                    LogMessage("  This means the XAML code-behind is not properly connected to the XAML markup.");
                }
            }
        }
        
        private void ReportEnvironment()
        {
            LogMessage("Environment information:");
            LogMessage($"- .NET Version: {Environment.Version}");
            LogMessage($"- OS Version: {Environment.OSVersion}");
            LogMessage($"- Directory: {Directory.GetCurrentDirectory()}");
        }
        
        private void LogMessage(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}";
            File.AppendAllText(_logPath, entry + Environment.NewLine);
        }
    }
}