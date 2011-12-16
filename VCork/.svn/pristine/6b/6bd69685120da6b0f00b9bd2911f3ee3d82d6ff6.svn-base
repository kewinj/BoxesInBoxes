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
using System.Windows.Resources;
using System.Windows.Media.Imaging;

namespace MyControlLibrary
{
    public partial class Corkboard : UserControl
    {

        public Corkboard()
        {
            InitializeComponent();

            this.MouseLeftButtonUp += new MouseButtonEventHandler(Corkboard_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Corkboard_MouseLeftButtonDown);
            StoryLimit = 8;
        }

        public int StoryLimit { get; set; }

        public void AddStory(SubGrid g)
        {
            if (Stories.Children.Count < StoryLimit)
            {
                Stories.Children.Add(g);
            }
        }

        public void RemoveStory()
        {
            if (Stories.Children.Count > 0)
            {
                Stories.Children.RemoveAt(0);
            }
        }

        #region MouseEvents

        void Corkboard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
        }

        void Corkboard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).ReleaseMouseCapture();
        }


        #endregion
    }
}
