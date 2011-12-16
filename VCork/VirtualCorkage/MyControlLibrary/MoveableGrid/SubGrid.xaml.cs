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

    /// <summary>
    /// Single master detail combination
    /// </summary>
    public partial class SubGrid : UserControl
    {
        private int _count;

        public SubGrid()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SubGrid_Loaded);            
        }
                
        void SubGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Tasks.Initialize(6, 2);
            Tasks.IsEditing = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddItem(Tasks.IconSize, _count);
            _count++;
        }

        private void AddItem(Point p, int pos)
        {
            TaskCard icon1 = new TaskCard()
            {
                Width = p.X,
                Height = p.Y
            };
            Tasks.AddIcon(icon1, pos);
        }
    }
}
