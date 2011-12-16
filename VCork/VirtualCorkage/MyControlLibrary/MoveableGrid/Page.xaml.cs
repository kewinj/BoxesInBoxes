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

namespace MyControlLibrary
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RearrangableCanvas.Initialize(CommonConstants.GRID_COLUMNS, CommonConstants.GRID_ROWS);

            /*
            for (int iconCount = 0; iconCount < CommonConstants.INITIAL_ICON_COUNT; iconCount++)
            {
                IconSample icon = new IconSample("calculator.png")
                {
                    Width = RearrangableCanvas.IconSize.X,
                    Height = RearrangableCanvas.IconSize.Y
                };
                RearrangableCanvas.AddIcon(icon, iconCount);
            }
            */

            AddItem("calculator.png", RearrangableCanvas.IconSize, 0);
            AddItem("calendar.png", RearrangableCanvas.IconSize, 1);
            AddItem("camera.png", RearrangableCanvas.IconSize, 2);
            AddItem("clock.png", RearrangableCanvas.IconSize, 3);
            AddItem("maps.png", RearrangableCanvas.IconSize, 4);
            AddItem("notes.png", RearrangableCanvas.IconSize, 5);
            AddItem("photos.png", RearrangableCanvas.IconSize, 6);
            AddItem("settings.png", RearrangableCanvas.IconSize, 7);
            AddItem("SMS.png", RearrangableCanvas.IconSize, 8);
            AddItem("stocks.png", RearrangableCanvas.IconSize, 9);
            AddItem("weather.png", RearrangableCanvas.IconSize, 10);
            //AddItem("Youtube.png", RearrangableCanvas.IconSize, 11);


            RearrangableCanvas.IsEditing = true;
        }

        private void AddItem(string path, Point p, int pos)
        {
            IconSample icon1 = new IconSample(path)
            {
                Width = p.X,
                Height = p.Y
            };
            RearrangableCanvas.AddIcon(icon1, pos);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            RearrangableCanvas.CancelChanges();
        }
    }
}
