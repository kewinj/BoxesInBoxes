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
using MyControlLibrary;
using System.Windows.Resources;
using System.Windows.Media.Imaging;

namespace VirtualCorkage
{
    public partial class MainPage : UserControl
    {

        public MainPage()
        {
            InitializeComponent();
            LayoutRoot.SizeChanged +=new SizeChangedEventHandler(LayoutRoot_SizeChanged);
        }

        private void LayoutRoot_SizeChanged(object s, SizeChangedEventArgs e) 
        {
            clipRegion.Rect  = new Rect(new Point(zoomPanel.ActualWidth , zoomPanel.ActualHeight ), new Point());
        }

        #region Story Related Methods

        private void AddButton_Click(object sender, RoutedEventArgs e)
        { 
            SubGrid sg = new SubGrid();
            corkBoard.AddStory(sg);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            corkBoard.RemoveStory();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            zoomPanel.ResetZoomAndPan();
        }

        #endregion
    }
}
