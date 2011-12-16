using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace DragDockPanelSample
{
    /// <summary>
    /// The drag dock panel sample.
    /// </summary>
    public partial class DragSample : UserControl
    {
        /// <summary>
        /// Stores a collection of the panels.
        /// </summary>
        private ObservableCollection<DragDockPanel> panels = new ObservableCollection<DragDockPanel>();

        /// <summary>
        /// Drag dock panel sample constructor.
        /// </summary>
        public DragSample()
        {
            this.InitializeComponent();

            this.dragDockPanelHost.ItemsSource = this.panels;

            //this.dragDockPanelHost.MaxColumns = 3;
            //this.dragDockPanelHost.MaxRows = 2;
            //this.dragDockPanelHost.MinimizedPosition = MinimizedPositions.Right;

            for (int i = 0; i < 6; i++)
            {
                this.AddPanel();
            }



            this.minimizedPositionComboBox.SelectionChanged += new SelectionChangedEventHandler(this.MinimizedPositionComboBox_SelectionChanged);
            this.addPanelButton.Click += new RoutedEventHandler(this.AddPanelButton_Click);
            this.removePanelButton.Click += new RoutedEventHandler(this.RemovePanelButton_Click);

            this.maxRows.TextChanged += new TextChangedEventHandler(this.MaxRows_TextChanged);
            this.maxColumns.TextChanged += new TextChangedEventHandler(this.MaxColumns_TextChanged);
        }

        /// <summary>
        /// Updates the max columns.
        /// </summary>
        /// <param name="sender">The max columns text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void MaxColumns_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxColumns = 0;
            int.TryParse(this.maxColumns.Text, out maxColumns);
            this.dragDockPanelHost.MaxColumns = maxColumns;
        }

        /// <summary>
        /// Updates the max rows.
        /// </summary>
        /// <param name="sender">The max rows text box.</param>
        /// <param name="e">Text changed event args.</param>
        private void MaxRows_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxRows = 0;
            int.TryParse(this.maxRows.Text, out maxRows);
            this.dragDockPanelHost.MaxRows = maxRows;
        }

        /// <summary>
        /// Adds a panel to the host.
        /// </summary>
        /// <param name="sender">The add button.</param>
        /// <param name="e">Routed Event Args.</param>
        private void AddPanelButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddPanel();
        }

        /// <summary>
        /// Adds a panel.
        /// </summary>
        private void AddPanel()
        {
             TextBlock tb = new TextBlock()
                {
                    Text = "C O N T E N T: This could be a sample task in our story board",
                    FontFamily = new FontFamily("Verdana"),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Foreground = new SolidColorBrush(Color.FromArgb(0x44, 255, 0, 255))
                    
                };
             tb.MouseLeftButtonDown += new MouseButtonEventHandler(DragSample_MouseLeftButtonDown);


            this.panels.Add(new DragDockPanel()
            {
                Margin = new Thickness(15),
                Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost.Items.Count + 1),
                
                Content =tb
            });
        }

        void DragSample_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            if (tb != null)
            {
                
                tb.Text = "Whoa! - you clicked on me...";

                DragDockPanel ddp = tb.Parent as DragDockPanel;
                if (ddp != null)
                {
                    TextBox tt = new TextBox();
                    tt.Text = tb.Text;
                    tt.TextWrapping = TextWrapping.Wrap;
                    tt.MouseLeave +=new MouseEventHandler(tt_MouseLeave);
                    ddp.Content = tt;
                }

            }
        }

        void tt_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBox tb2 = sender as TextBox;
            if (tb2 != null)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = tb2.Text,
                    FontFamily = new FontFamily("Verdana"),
                    FontSize = 14,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = System.Windows.TextWrapping.Wrap, 
                    Foreground = new SolidColorBrush(Color.FromArgb(0x44, 255, 0, 255))

                };
                tb.MouseLeftButtonDown += new MouseButtonEventHandler(DragSample_MouseLeftButtonDown);

                DragDockPanel ddp = tb2.Parent as DragDockPanel;
                if (ddp != null)
                {
                    ddp.Content = tb;
                }

                
            }
        }

        /// <summary>
        /// Removes a panel from the host.
        /// </summary>
        /// <param name="sender">Remove panel button.</param>
        /// <param name="e">Routed event args.</param>
        private void RemovePanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.panels.Count > 0)
            {
                this.panels.RemoveAt(this.panels.Count - 1);
            }
        }

        /// <summary>
        /// Updates the minimised position.
        /// </summary>
        /// <param name="sender">The minimized combo box.</param>
        /// <param name="e">Selection changed event args.</param>
        private void MinimizedPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.minimizedPositionComboBox.SelectedIndex)
            {
                case 0:
                    this.dragDockPanelHost.MinimizedPosition = MinimizedPositions.Right;
                    break;
                case 1:
                    this.dragDockPanelHost.MinimizedPosition = MinimizedPositions.Bottom;
                    break;
                case 2:
                    this.dragDockPanelHost.MinimizedPosition = MinimizedPositions.Left;
                    break;
                case 3:
                    this.dragDockPanelHost.MinimizedPosition = MinimizedPositions.Top;
                    break;
            }
        }
    }
}
