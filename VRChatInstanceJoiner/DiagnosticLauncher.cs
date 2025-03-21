using System;
using System.Windows;

namespace VRChatInstanceJoiner
{
    /// <summary>
    /// Entry point for running the diagnostic tools
    /// </summary>
    public class DiagnosticLauncher
    {
        public static void RunDiagnostics()
        {
            try
            {
                // Create and run the diagnostic app
                var app = new DiagnosticApp();
                app.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error launching diagnostic app: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}