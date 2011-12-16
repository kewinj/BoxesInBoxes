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
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace MyControlLibrary
{
    public partial class IconSample : UserControl, IIcon
    {        
        private string _iconPath;

        public IconSample(string iconPath)
        {
            _iconPath = iconPath;

            InitializeComponent();

            this.Loaded += new RoutedEventHandler(IconSample_Loaded);
        }

        void IconSample_Loaded(object sender, RoutedEventArgs e)
        {
            StreamResourceInfo sr = Application.GetResourceStream(
                new Uri(string.Format("MyControlLibrary;component/Resources/{0}", _iconPath),
                    UriKind.Relative));

            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(sr.Stream);

            iconImage.Source = bmp;

            LayoutRoot.Width = this.Width;
            LayoutRoot.Height = this.Height;
        }

    }
}
