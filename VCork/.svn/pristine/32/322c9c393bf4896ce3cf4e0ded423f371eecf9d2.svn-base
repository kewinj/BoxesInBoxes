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
using System.Windows.Data;

namespace MyControlLibrary
{
    public partial class TaskCard : UserControl, IIcon
    {
        private DateTime _clickedDown;
        private bool _isClicked;
        private bool _isActive;

        public TaskCard()
        {
            InitializeComponent();

            _isActive = false;

            taskTextBlock.SetBinding(TextBlock.TextProperty, new Binding("Text") { Source = this, Mode = BindingMode.TwoWay });

            MouseLeftButtonDown += TaskCardMouseLeftButtonDown;
            MouseLeftButtonUp +=  TaskCardMouseLeftButtonUp;
            MouseLeave +=  TaskCardMouseLeave;  
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TaskCard), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        void TaskCardMouseLeave(object sender, MouseEventArgs e)
        {
            if (_isActive)
            {
                taskTextBlock.Visibility  = System.Windows.Visibility.Visible;
                taskTextBox.Visibility = System.Windows.Visibility.Collapsed;
                taskTextBlock.Text = taskTextBox.Text;
                _isActive = false;
            }
        }

        void TaskCardMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).ReleaseMouseCapture();
            TimeSpan span = DateTime.Now - _clickedDown;
            if (span.TotalMilliseconds < 300 && _isClicked)
            {
                //thus user performed a single click on the card...
                taskTextBlock.Visibility  = System.Windows.Visibility.Collapsed;
                taskTextBox.Visibility = System.Windows.Visibility.Visible;
                taskTextBox.Text = taskTextBlock.Text;
                _isActive = true;
            }
            _isClicked = false;
        }

        void TaskCardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            _clickedDown = DateTime.Now;
            _isClicked = true;
            
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
