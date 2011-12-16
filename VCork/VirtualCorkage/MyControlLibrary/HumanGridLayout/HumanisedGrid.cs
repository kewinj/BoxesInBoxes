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
using MyControlLibrary.HumanGridLayout;

namespace MyControlLibrary
{
    public static class SizeExtentions
    {
        public static double Area(this Size size)
        {
            return size.Height*size.Width;
        }

        public static Size Multiply(this Size size, double multiplier)
        {
            return new Size(size.Height * multiplier, size.Width * multiplier);
        }

        public static Size Double(this Size size)
        {
            return size.Multiply(2);
        }
    }

    public class HumanisedGrid : Panel
    {
        public HumanisedGrid(){}

        private static Size Infinite = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
        private double _tileWidth;
        private double _tileHeight;
        private Size _tileSpacing = new Size(52, 30.90);

        protected override Size MeasureOverride(Size availableSize)
        {
            if(Children.Count == 0)
            {
                return new Size(0, 0);
            }
            var crp = new CygonRectanglePacker(availableSize.Width, availableSize.Height);

            foreach(UIElement element in Children)
            {
                element.Measure(Infinite);
                Point placement;
                crp.TryPack(element.DesiredSize.Width, element.DesiredSize.Height, out placement);
            }
            
            //var sizedElements = Children.OrderBy(child => child.DesiredSize.Area()).Reverse.ToList();

            _tileWidth = Children
                .Where(child => child.Visibility != Visibility.Collapsed)
                .Max(child => child.DesiredSize.Width);

            _tileHeight = Children
                .Where(child => child.Visibility != Visibility.Collapsed)
                .Max(child => child.DesiredSize.Height);

            double top = 0;
            double left = 0;

            bool first = true;
            foreach(UIElement element in Children)
            {
                if(element.Visibility != Visibility.Collapsed)
                {
                    if(first == false)
                    {
                        left += _tileSpacing.Width;
                    }

                    left += _tileWidth;

                    if((left + _tileSpacing.Width + _tileWidth) > availableSize.Width)
                    {
                        left = 0;
                        top += _tileHeight + _tileSpacing.Height;
                        first = true;
                    }
                    else
                    {
                        first = false;
                    }
                }
            }

            if(left == 0)
            {
                return new Size(availableSize.Width, top);
            }
            //return new Size(double.IsPositiveInfinity(availableSize.Width) ? _tileWidth*4 : availableSize.Width, top + _tileHeight);
            return new Size(
                double.IsPositiveInfinity(availableSize.Width) ? _tileWidth*3.5 : availableSize.Width,
                double.IsPositiveInfinity(availableSize.Height) ? _tileHeight * 2 : availableSize.Height);
        }

        private Random _rnd = new Random();
        private CygonRectanglePacker _crp;

        protected override Size ArrangeOverride(Size finalSize)
        {
            if(Children.Count != 0)
            {
                double top = 0;
                double left = 0;
                double width = _tileWidth;
                double height = _tileHeight;
                Size spacing = _tileSpacing;
                bool first = true;
                var crp = new CygonRectanglePacker(finalSize.Width, finalSize.Height);

                var sizedElements = Children.OrderBy(child => child.DesiredSize.Area()).Reverse();
                foreach(UIElement element in sizedElements)
                {
                    if(element.Visibility != Visibility.Collapsed)
                    {
                        Point placement;
                        crp.TryPack(element.DesiredSize.Width, element.DesiredSize.Height, out placement);
                        Rect finalRect = new Rect(placement, element.DesiredSize);
                        element.Arrange(finalRect);
                        //element.SetValue(BoundsProperty, finalRect);
                        //if(first == false)
                        //{
                        //    left += spacing.Width;
                        //}
                        //ArrangeElement(element, new Rect(left, top, width, height));
                        //left = left + width;

                        //if((left + spacing.Width + width) > finalSize.Width)
                        //{
                        //    left = 0;
                        //    top += height + spacing.Height;
                        //    first = true;
                        //}
                        //else
                        //{
                        //    first = false;
                        //}
                    }
                    element.RenderTransform = new CompositeTransform()
                                                  {
                                                      Rotation = _rnd.Next(-5, 5),
                                                      CenterX = element.DesiredSize.Width / 2,
                                                      CenterY = element.DesiredSize.Height / 2,
                                                      //TranslateX = _rnd.Next((int)-(element.DesiredSize.Width / 10), (int)(element.DesiredSize.Width / 10)),
                                                      //TranslateY = _rnd.Next((int)-(element.DesiredSize.Height / 10), (int)(element.DesiredSize.Height / 10))
                                                  };
                }
            }

            return finalSize;
        }
    }
}
