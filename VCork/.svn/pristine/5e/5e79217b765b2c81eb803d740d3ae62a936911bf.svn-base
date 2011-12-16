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
    public class TransitionHelper
    {
        public static TransformGroup GetTransformGroupForXYOffset(double X, double Y, double scaleX, double scaleY)
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = X;
            tt.Y = Y;

            RotateTransform rt = new RotateTransform();
            rt.Angle = 0.00D;

            ScaleTransform st = new ScaleTransform();
            st.ScaleX = scaleX;
            st.ScaleY = scaleY;

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(st);
            tg.Children.Add(rt);
            tg.Children.Add(tt);            
            
            return tg;
        }

        public static Storyboard BuildTranslateTransformTransition(UIElement element, Point newLocation, TimeSpan period, bool autoReverse)
        {
            Duration duration = new Duration(period);

            //ElasticEase ease = new ElasticEase();
            //ease.EasingMode = EasingMode.EaseOut;
            //ease.Oscillations = 2;
            //ease.Springiness = 6;

            // Animate X
            DoubleAnimation translateAnimationX = new DoubleAnimation();
            translateAnimationX.To = newLocation.X;
            translateAnimationX.Duration = duration;
            //translateAnimationX.EasingFunction = ease;

            Storyboard.SetTarget(translateAnimationX, element);
            Storyboard.SetTargetProperty(translateAnimationX,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(TranslateTransform.X)"));

            // Animate Y
            DoubleAnimation translateAnimationY = new DoubleAnimation();
            translateAnimationY.To = newLocation.Y;
            translateAnimationY.Duration = duration;
            //translateAnimationY.EasingFunction = ease;

            Storyboard.SetTarget(translateAnimationY, element);
            Storyboard.SetTargetProperty(translateAnimationY,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(TranslateTransform.Y)"));

            Storyboard sb = new Storyboard();
            sb.AutoReverse = autoReverse;
            sb.Duration = duration;
            sb.Children.Add(translateAnimationX);
            sb.Children.Add(translateAnimationY);

            return sb;
        }
    }
}
