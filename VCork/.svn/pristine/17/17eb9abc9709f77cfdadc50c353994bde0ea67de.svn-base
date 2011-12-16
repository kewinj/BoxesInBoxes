using System;
using System.Windows;
using System.Windows.Controls;

namespace VCork
{
    public partial class Task : UserControl
    {
        public Task()
        {
            InitializeComponent();
        }

        public String TaskText
        {
            get { return (String)GetValue(TaskTextProperty); }
            set { SetValue(TaskTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskTextProperty = DependencyProperty.Register("TaskText", typeof(String), typeof(Task), new PropertyMetadata(TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Task) d).UpdateText(e.NewValue.ToString());
        }

        private void UpdateText(string text)
        {
            taskText.Text = text;
        }
    }
}

