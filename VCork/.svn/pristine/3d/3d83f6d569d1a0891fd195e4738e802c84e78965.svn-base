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

namespace VCork
{
    public partial class StoryCard : UserControl
    {
        public StoryCard()
        {
            InitializeComponent();
        }

        public String StoryText
        {
            get { return (String)GetValue(TaskTextProperty); }
            set { SetValue(TaskTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskTextProperty = DependencyProperty.Register("StoryText", typeof(String), typeof(StoryCard), new PropertyMetadata(TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StoryCard)d).UpdateText(e.NewValue.ToString());
        }

        private void UpdateText(string text)
        {
            storyText.Text = text;
        }
    }
}
