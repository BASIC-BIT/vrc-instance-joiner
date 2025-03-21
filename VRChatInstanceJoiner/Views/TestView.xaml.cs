using System.Windows;
using System.Windows.Controls;
using VRChatInstanceJoiner.ViewModels;

namespace VRChatInstanceJoiner.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            // This is the critical line missing in your other views
            InitializeComponent();
            
            // Create and set the ViewModel
            var viewModel = new TestViewModel();
            DataContext = viewModel;
            
            // Example of direct access to a XAML element
            // This shows both binding and direct manipulation can work together
            DirectlyAccessedElement.Text = "Element accessed from code-behind!";
        }
    }
}