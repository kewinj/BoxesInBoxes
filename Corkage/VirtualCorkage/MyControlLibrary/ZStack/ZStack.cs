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
	public class ZStack : Panel
	{
		private Random rnd = new Random();

		#region MaxRotation (DependencyProperty)

		public double MaxRotation
		{
			get
			{
				return (double)GetValue(MaxRotationProperty);
			}
			set
			{
				SetValue(MaxRotationProperty, value);
			}
		}

		public static readonly DependencyProperty MaxRotationProperty = DependencyProperty.Register("MaxRotation", typeof(double), typeof(ZStack), new PropertyMetadata(0.0, OnMaxRotationChanged));


		private static void OnMaxRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ZStack)d).OnMaxRotationChanged(e);
		}

		protected virtual void OnMaxRotationChanged(DependencyPropertyChangedEventArgs e)
		{ }

		#endregion MaxRotation

		#region MaxXOffset (DependencyProperty)
		public double MaxXOffset
		{
			get
			{
				return (double)GetValue(MaxXOffsetProperty);
			}
			set
			{
				SetValue(MaxXOffsetProperty, value);
			}
		}

		public static readonly DependencyProperty MaxXOffsetProperty = DependencyProperty.Register("MaxXOffset", typeof(double), typeof(ZStack), new PropertyMetadata(0.0, OnMaxXOffsetChanged));


		private static void OnMaxXOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ZStack)d).OnMaxXOffsetChanged(e);
		}

		protected virtual void OnMaxXOffsetChanged(DependencyPropertyChangedEventArgs e) { }
		#endregion

		#region MaxYOffset (DependencyProperty)
		public double MaxYOffset
		{
			get
			{
				return (double)GetValue(MaxYOffsetProperty);
			}
			set
			{
				SetValue(MaxYOffsetProperty, value);
			}
		}

		public static readonly DependencyProperty MaxYOffsetProperty = DependencyProperty.Register("MaxYOffset", typeof(double), typeof(ZStack), new PropertyMetadata(0.0, OnMaxYOffsetChanged));


		private static void OnMaxYOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ZStack)d).OnMaxYOffsetChanged(e);
		}

		protected virtual void OnMaxYOffsetChanged(DependencyPropertyChangedEventArgs e)
		{
		}
		#endregion

		public ZStack() : base()
		{
				
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size b = base.MeasureOverride(availableSize);
			Size maxChildSize = new Size(0, 0);
			foreach (UIElement child in Children)
			{
				child.Measure(availableSize);
				maxChildSize.Width = Math.Max(maxChildSize.Width, child.DesiredSize.Width);
				maxChildSize.Height = Math.Max(maxChildSize.Height, child.DesiredSize.Height);
			 }

			maxChildSize.Width = double.IsPositiveInfinity(availableSize.Width) ? maxChildSize.Width : availableSize.Width;

			maxChildSize.Height = double.IsPositiveInfinity(availableSize.Height) ? maxChildSize.Height : availableSize.Height;
            //reported size should be the largest child translated and rotaled to the maximum bounds...
		    return maxChildSize.Double();
		}

		private bool done;
		protected override Size ArrangeOverride(Size finalSize)
		{
			foreach (UIElement child in Children)
			{
				double childX = finalSize.Width / 2 - child.DesiredSize.Width / 2;
				double childY = finalSize.Height / 2 - child.DesiredSize.Height / 2;
				if(! done)
				{
					RotateAndOffsetChild(child);
				}
				
				child.Arrange(new Rect(childX, childY, child.DesiredSize.Width, child.DesiredSize.Height));

			}
			if(!done)
			{
				done = true;
			}
			return finalSize;
		}

		private void RotateAndOffsetChild(UIElement child)
		{
			double xOffset = MaxXOffset * (2 * rnd.NextDouble() - 1);
			double yOffset = MaxYOffset * (2 * rnd.NextDouble() - 1);
			double angle = MaxRotation * (2 * rnd.NextDouble() - 1);

		    var ct = new CompositeTransform()
		                                {
		                                    Rotation = angle,
		                                    CenterX = child.DesiredSize.Width/2,
		                                    CenterY = child.DesiredSize.Height/2,
		                                    TranslateX = xOffset,
		                                    TranslateY = yOffset
		                                };
			child.RenderTransform = ct;
		}
	}
}
