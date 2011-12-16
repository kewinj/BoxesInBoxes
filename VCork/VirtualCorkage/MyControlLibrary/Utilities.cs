using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyControlLibrary
{
    public static class Extensions
    {
        public static void SetClip(this FrameworkElement element)
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Clip = new RectangleGeometry
            {
                Rect = new Rect
                (
                    0, 0,
                    element.ActualWidth,
                    element.ActualHeight
                )
            };
        }

        public static Point Subtract(this Point a, Point b)
        {
            Point ret = new Point();
            ret.X = a.X - b.X;
            ret.Y = a.Y - b.Y;
            return ret;
        }
    }
}
