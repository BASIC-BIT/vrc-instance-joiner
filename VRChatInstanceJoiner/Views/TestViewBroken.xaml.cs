using System.Windows;
using System.Windows.Controls;
using VRChatInstanceJoiner.ViewModels;

namespace VRChatInstanceJoiner.Views
{
    /// <summary>
    /// Interaction logic for TestViewBroken.xaml
    /// </summary>
    public partial class TestViewBroken : UserControl
    {
        public TestViewBroken()
        {
            // Problem #1: Missing InitializeComponent() call
            // This is what connects the XAML to this code-behind
            
            // Create and set the ViewModel (this part is correct)
            var viewModel = new TestViewModel();
            DataContext = viewModel;
            
            // Problem #2: We try to access an element defined in XAML
            // But since InitializeComponent() wasn't called, it doesn't exist in code
            try {
                // This will fail with NullReferenceException
                DirectlyAccessedElement.Text = "This will crash!";
            }
            catch (System.Exception ex) {
                MessageBox.Show($"Error: {ex.Message}\n\nThis occurred because InitializeComponent() wasn't called.");
            }
        }
    }
}