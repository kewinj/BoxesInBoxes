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
    [TemplatePart(Name = CONTAINER, Type = typeof(Canvas))]
    [TemplatePart(Name = ZOOMSTORYBOARD, Type = typeof(Storyboard))]
    [TemplatePart(Name = ZOOMX, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = ZOOMY, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = PANX, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = PANY, Type = typeof(DoubleAnimation))]
    public class ZoomPanel : ContentControl
    {
        #region Private Members

        private const string CONTAINER = "ZoomPanelContainer";
        private const string ZOOMSTORYBOARD = "ZoomStoryboard";
        private const string ZOOMX = "ZoomX";
        private const string ZOOMY = "ZoomY";
        private const string PANX = "PanX";
        private const string PANY = "PanY";

        private Canvas _container;
        private CompositeTransform _transform;
        private Storyboard _zoomStoryboard;
        private DoubleAnimation _zoomX;
        private DoubleAnimation _zoomY;
        private DoubleAnimation _panX;
        private DoubleAnimation _panY;

        private double _currentZoom;
        private Point _currentOffset;
        private Point _originalMousePosition;
        private Point _deltaMousePosition;

        private bool _mousePanning;
        private bool _mouseZooming;
        private bool _panningWhilstZooming;

        //private UIElement _originalSource;

        #endregion

        #region Public Properties

        public bool PanEnabled { get; set; }
        public bool ZoomEnabled { get; set; }
        public double MinZoom { get; set; }
        public double MaxZoom { get; set; }
        public double ZoomStep { get; set; }
        public double InitialZoom { get; set; }


        #region Lag Dependency Property
        public static readonly DependencyProperty LagProperty =
            DependencyProperty.Register("Lag", typeof(int), typeof(ZoomPanel),
            new PropertyMetadata(new PropertyChangedCallback(ZoomPanel.OnLagPropertyChanged)));

        public int Lag
        {
            get { return (int)GetValue(LagProperty); }
            set { SetValue(LagProperty, value); }
        }

        private static void OnLagPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomPanel panel = d as ZoomPanel;
            panel.OnLagChange();
        }

        private void OnLagChange()
        {
            //change the duration values on each of the navigation animations
            TimeSpan lagTimeSpan = new TimeSpan(0,0,0,0,Lag);
            if (_zoomX != null) _zoomX.Duration = new Duration(lagTimeSpan);
            if (_zoomY != null) _zoomY.Duration = new Duration(lagTimeSpan);
            if (_panX != null) _panX.Duration = new Duration(lagTimeSpan);
            if (_panY != null) _panY.Duration = new Duration(lagTimeSpan);
        }

        #endregion

        #endregion

        #region Construction of the Zoom Panel

        public ZoomPanel()
        {
            DefaultStyleKey = typeof(ZoomPanel);
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            PanEnabled = true;
            ZoomEnabled = true;
            InitialZoom = 0.1;
            MinZoom = 0.1;
            MaxZoom = 3;
            ZoomStep = 0.1;
            _currentOffset = new Point(0, 0);
            _deltaMousePosition = new Point(0, 0);
            _mouseZooming = false;
            _mousePanning = false;
            _panningWhilstZooming = false;
        }

        public override void OnApplyTemplate()
        {
            _container = GetTemplateChild(CONTAINER) as Canvas;
            _zoomStoryboard = GetTemplateChild(ZOOMSTORYBOARD) as Storyboard;
            _zoomX = GetTemplateChild(ZOOMX) as DoubleAnimation;
            _zoomY = GetTemplateChild(ZOOMY) as DoubleAnimation;
            _panX = GetTemplateChild(PANX) as DoubleAnimation;
            _panY = GetTemplateChild(PANY) as DoubleAnimation;
            OnLagChange();
            InitialiseTransform();

            base.OnApplyTemplate();
        }

        private void InitialiseTransform()
        {
            _transform = _container.RenderTransform as CompositeTransform;
            if (_transform == null)
            {
                throw new InvalidCastException("Unable to obtain CompositeTransform from " + CONTAINER);
            }

            if (InitialZoom < MinZoom) { InitialZoom = MinZoom; }

            _zoomStoryboard.Completed += new EventHandler(_zoomStoryboard_Completed);
            _currentZoom = InitialZoom;
        }

        private bool _storyboardActive = false;
        void _zoomStoryboard_Completed(object sender, EventArgs e)
        {
            _storyboardActive = false;
        }

        #endregion

        #region Mouse Events

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!PanEnabled) return;

            _mousePanning = true;
            _originalMousePosition = e.GetPosition(null);
            _currentOffset.X = _currentOffset.X - _deltaMousePosition.X;
            _currentOffset.Y = _currentOffset.Y - _deltaMousePosition.Y;

            //not sure if this is required
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (!PanEnabled) return;
            _mousePanning = false;
            e.Handled = false;

            //not sure if this is required
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_mousePanning || !PanEnabled) { return; }

            Point currentMousePosition = e.GetPosition(null);
            _deltaMousePosition = _originalMousePosition.Subtract(currentMousePosition);

            _panX.To = _currentOffset.X - _deltaMousePosition.X;
            _panY.To = _currentOffset.Y - _deltaMousePosition.Y;
            _zoomStoryboard.Begin();

            //must have been zooming before we panned
            if (_storyboardActive) _panningWhilstZooming = true;

            //not sure if this is required
            base.OnMouseMove(e);

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            //don't allow zooming if panning
            if (!ZoomEnabled || _mousePanning) return;

            double previousZoom = _currentZoom;

            if (e.Delta > 0)
            {
                _currentZoom += ZoomStep;
            }
            else
            {
                _currentZoom -= ZoomStep;
            }
            if (_currentZoom > MaxZoom)
                _currentZoom = MaxZoom;
            if (_currentZoom < MinZoom)
                _currentZoom = MinZoom;

            _currentZoom = Math.Round(_currentZoom, 2);
            double m11 = _currentZoom;
            double m22 = _currentZoom;

            Point mousePos = e.GetPosition(this);

            if (!_storyboardActive || _panningWhilstZooming)
            {
                //takes snap shot of board location
                MatrixTransform relativeTransform = (MatrixTransform)_container.TransformToVisual(this);
                double deltaX = mousePos.X - relativeTransform.Matrix.OffsetX;
                double deltaY = mousePos.Y - relativeTransform.Matrix.OffsetY;
                double offsetX = relativeTransform.Matrix.OffsetX + deltaX * (1 - _currentZoom / previousZoom);
                double offsetY = relativeTransform.Matrix.OffsetY + deltaY * (1 - _currentZoom / previousZoom);
                _currentOffset.X = offsetX;
                _currentOffset.Y = offsetY;
                _zoomX.To = _currentZoom;
                _zoomY.To = _currentZoom;
                _panX.To = _currentOffset.X;
                _panY.To = _currentOffset.Y;
                _deltaMousePosition.X = 0;
                _deltaMousePosition.Y = 0;
                _zoomStoryboard.Begin();
                _storyboardActive = true;
                _mouseZooming = true;
                _panningWhilstZooming = false;
            }
            else
            {
                //storyboard is currently running - so use the offset of where we want toend up after the animation has finshed
                double deltaX = mousePos.X - _currentOffset.X;
                double deltaY = mousePos.Y - _currentOffset.Y;
                double offsetX = _currentOffset.X + deltaX * (1 - _currentZoom / previousZoom);
                double offsetY = _currentOffset.Y + deltaY * (1 - _currentZoom / previousZoom);
                _currentOffset.X = offsetX;
                _currentOffset.Y = offsetY;
                _zoomX.To = _currentZoom;
                _zoomY.To = _currentZoom;
                _panX.To = _currentOffset.X;
                _panY.To = _currentOffset.Y;
                _deltaMousePosition.X = 0;
                _deltaMousePosition.Y = 0;
                _zoomStoryboard.Begin();

            }

            //not sure if this is required
            base.OnMouseWheel(e);
        }

        #endregion


        public void ResetZoomAndPan()
        {
            _currentZoom = 1;
            _currentOffset.X =0;
            _currentOffset.Y = 0;
            _zoomX.To = _currentZoom;
            _zoomY.To = _currentZoom;
            _panX.To = _currentOffset.X;
            _panY.To = _currentOffset.Y;
            _deltaMousePosition.X = 0;
            _deltaMousePosition.Y = 0;
            _zoomStoryboard.Begin();
     
        }
    }
}
