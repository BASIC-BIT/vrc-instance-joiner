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
            // Create a simple UI programmatically
            CreateUI();
        }

        private void CreateUI()
        {
            // Create a Grid as the main container
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var headerText = new TextBlock
            {
                Text = "Group Selection",
                FontSize = 24,
                Margin = new Thickness(0, 0, 0, 16)
            };
            Grid.SetRow(headerText, 0);
            grid.Children.Add(headerText);

            // Search Box
            var searchGrid = new Grid();
            searchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            searchGrid.Margin = new Thickness(0, 0, 0, 16);

            var searchBox = new TextBox
            {
                Margin = new Thickness(0, 0, 8, 0)
            };
            searchBox.SetBinding(TextBox.TextProperty, new Binding("SearchText") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            Grid.SetColumn(searchBox, 0);
            searchGrid.Children.Add(searchBox);

            var refreshButton = new Button
            {
                Content = "Refresh"
            };
            refreshButton.SetBinding(Button.CommandProperty, new Binding("RefreshGroupsCommand"));
            Grid.SetColumn(refreshButton, 1);
            searchGrid.Children.Add(refreshButton);

            Grid.SetRow(searchGrid, 1);
            grid.Children.Add(searchGrid);

            // Group List
            var listView = new ListView();
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding("FilteredGroups"));
            listView.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedGroup"));
            
            // Create a simple item template
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            
            var template = new DataTemplate();
            template.VisualTree = factory;
            
            listView.ItemTemplate = template;
            
            Grid.SetRow(listView, 2);
            grid.Children.Add(listView);

            // Status Bar
            var statusGrid = new Grid();
            statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            statusGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var statusText = new TextBlock();
            statusText.SetBinding(TextBlock.TextProperty, new Binding("StatusMessage"));
            Grid.SetColumn(statusText, 0);
            statusGrid.Children.Add(statusText);

            var selectButton = new Button
            {
                Content = "Select Group"
            };
            selectButton.SetBinding(Button.CommandProperty, new Binding("SelectGroupCommand"));
            selectButton.SetBinding(Button.CommandParameterProperty, new Binding("SelectedGroup"));
            Grid.SetColumn(selectButton, 1);
            statusGrid.Children.Add(selectButton);

            Grid.SetRow(statusGrid, 3);
            grid.Children.Add(statusGrid);

            // Set the content
            Content = grid;
        }
    }
}