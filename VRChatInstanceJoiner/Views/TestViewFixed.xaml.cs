using System.Windows;
using System.Windows.Controls;
using VRChatInstanceJoiner.ViewModels;

namespace VRChatInstanceJoiner.Views
{
    /// <summary>
    /// Interaction logic for TestViewFixed.xaml
    /// </summary>
    public partial class TestViewFixed : UserControl
    {
        public TestViewFixed()
        {
            // Solution #1: Call InitializeComponent() first
            // This loads the XAML, connects named elements, and sets up the UI
            InitializeComponent();
            
            // Solution #2: Create and set the ViewModel after InitializeComponent
            var viewModel = new TestViewModel();
            DataContext = viewModel;
            
            // Solution #3: Now we can safely access XAML-defined elements
            // This works because InitializeComponent() was called
            DirectlyAccessedElement.Text = "Successfully accessed from code-behind!";
            
            // Note: We're NOT creating UI programmatically here
            // We're letting XAML handle all UI definition and just
            // binding to the ViewModel and accessing named elements as needed
        }
    }
}