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
    [TemplatePart(Name = Container, Type = typeof(Grid))]
    [TemplatePart(Name = Zoomstoryboard, Type = typeof(Storyboard))]
    [TemplatePart(Name = Zoomx, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = Zoomy, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = Panx, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = Pany, Type = typeof(DoubleAnimation))]
    public class ZoomPanel : ContentControl
    {
        #region Private Members

        private const string Container = "ZoomPanelContainer";
        private const string Zoomstoryboard = "ZoomStoryboard";
        private const string Zoomx = "ZoomX";
        private const string Zoomy = "ZoomY";
        private const string Panx = "PanX";
        private const string Pany = "PanY";

        private Grid _container;
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

        #endregion

        #region Public Properties

        public bool PanEnabled { get; set; }
        public bool ZoomEnabled { get; set; }
        public double MinZoom { get; set; }
        public double MaxZoom { get; set; }
        public double ZoomStep { get; set; }
        public double InitialZoom { get; set; }

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
            InitialZoom = 1;
            MinZoom = 1;
            MaxZoom = 3;
            ZoomStep = 1;
            _currentOffset = new Point(0, 0);
            _deltaMousePosition = new Point(0, 0);
            _mouseZooming = false;
            _mousePanning = false;
            _panningWhilstZooming = false;
        }

        public override void OnApplyTemplate()
        {
            _container = GetTemplateChild(Container) as Grid;
            _zoomStoryboard = GetTemplateChild(Zoomstoryboard) as Storyboard;
            _zoomX = GetTemplateChild(Zoomx) as DoubleAnimation;
            _zoomY = GetTemplateChild(Zoomy) as DoubleAnimation;
            _panX = GetTemplateChild(Panx) as DoubleAnimation;
            _panY = GetTemplateChild(Pany) as DoubleAnimation;

            InitialiseTransform();

            base.OnApplyTemplate();
        }

        private void InitialiseTransform()
        {

            _transform = _container.RenderTransform as CompositeTransform;
            if (_transform == null)
            {
                throw new InvalidCastException("Unable to obtain CompositeTransform from " + Container);
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
            CaptureMouse();
           // 
            _mousePanning = true;
            _originalMousePosition = e.GetPosition(null);

            _currentOffset.X = _currentOffset.X - _deltaMousePosition.X;
            _currentOffset.Y = _currentOffset.Y - _deltaMousePosition.Y;

            //e.Handled = true;
            base.OnMouseLeftButtonDown(e);
         
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (!PanEnabled) return;
            ReleaseMouseCapture();
         //   
            _mousePanning = false;
            e.Handled = false;
            base.OnMouseLeftButtonUp(e);
          
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_mousePanning || !PanEnabled)
                return;

            Point currentMousePosition = e.GetPosition(null);
            _deltaMousePosition = _originalMousePosition.Subtract(currentMousePosition);

            _panX.To = _currentOffset.X - _deltaMousePosition.X;
            _panY.To = _currentOffset.Y - _deltaMousePosition.Y;
            _zoomStoryboard.Begin();

            //must have been zooming before we panned
            if (_storyboardActive) _panningWhilstZooming = true;

            base.OnMouseMove(e);

        }



        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {

            if (!ZoomEnabled) return;

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

            base.OnMouseWheel(e);
        }

        #endregion

    }
}
