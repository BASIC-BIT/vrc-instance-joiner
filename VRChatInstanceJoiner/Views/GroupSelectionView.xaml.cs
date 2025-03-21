using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VRChatInstanceJoiner.ViewModels;

namespace VRChatInstanceJoiner.Views
{
    /// <summary>
    /// Interaction logic for GroupSelectionView.xaml
    /// </summary>
    public partial class GroupSelectionView : UserControl
    {
        public GroupSelectionView()
        {
            // Call InitializeComponent to load and connect the XAML UI
            // This replaces the CreateUI() approach which was competing with XAML
            InitializeComponent();
            
            // Any additional initialization can go here
            // but we no longer need to create UI elements programmatically
        }
    }
}