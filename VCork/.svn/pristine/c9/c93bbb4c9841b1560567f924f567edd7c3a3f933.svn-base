using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DragDockPanelSample;

namespace VCork
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.dragDockPanelHost1.ItemsSource = this.panels1;
            this.dragDockPanelHost2.ItemsSource = this.panels2;
            this.dragDockPanelHost3.ItemsSource = this.panels3;
            this.dragDockPanelHost4.ItemsSource = this.panels4;
            for (int i = 0; i < 15; i++)
            {
                this.AddPanel();
            }
        }

        private double _zoom =1d;
        private bool _mousePanning;
        private Point _mousePosition;
        private Point _previousmousePosition;
        private double _xTransform;
        private double _yTransform;
        private ObservableCollection<DragDockPanel> panels1 = new ObservableCollection<DragDockPanel>();
        private ObservableCollection<DragDockPanel> panels2 = new ObservableCollection<DragDockPanel>();
        private ObservableCollection<DragDockPanel> panels3 = new ObservableCollection<DragDockPanel>();
        private ObservableCollection<DragDockPanel> panels4 = new ObservableCollection<DragDockPanel>();

        private void AddPanel()
        {
            this.panels1.Add(new DragDockPanel()
                                {
                                    Margin = new Thickness(10),
                                    Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost1.Items.Count + 1),
                                    Content = new Task { TaskText = "As a user I want movable stuff to be moved around with the mouse." }
                                });

            this.panels2.Add(new DragDockPanel()
            {
                Margin = new Thickness(10),
                Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost2.Items.Count + 1),
                Content = new Task { TaskText = "As a user I want stuff thats cool." }
            });

            this.panels3.Add(new DragDockPanel()
            {
                Margin = new Thickness(10),
                Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost3.Items.Count + 1),
                Content = new Task { TaskText = "As a user I want no bugs." }
            });

            this.panels4.Add(new DragDockPanel()
            {
                Margin = new Thickness(10),
                Header = string.Format("{0} {1}", "Panel", this.dragDockPanelHost4.Items.Count + 1),
                Content = new Task { TaskText = "As a user I want to go home early." }
            });
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            double deltaZoom = e.Delta / 600d;
            _zoom += deltaZoom;
            if (_zoom <= 0.1)
            {
                _zoom = 0.1;
            }
            if (_zoom > 50)
            {
                _zoom = 50;
            }

            ZoomAnimationX.To = _zoom;
            ZoomAnimationY.To = _zoom;
            ZoomCenterAnimationX.To = Corkboard.Width / 2;
            ZoomCenterAnimationY.To = Corkboard.Height / 2;
            ZoomStoryboard.Begin();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _previousmousePosition = _mousePosition;
            _mousePosition = e.GetPosition(this);

            if (_mousePanning)
            {
                Pan(_previousmousePosition, _mousePosition);
            }
        }

        private void Pan(Point previousmousePosition, Point mousePosition)
        {
            _xTransform -= (previousmousePosition.X - mousePosition.X);
            _yTransform -= (previousmousePosition.Y - mousePosition.Y);
            TranslateX.To = _xTransform;
            TranslateY.To = _yTransform;
            TranslateStoryboard.Begin();
        }

        private void UserControlMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            _mousePanning = true;
        }

        private void UserControlMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse
            ((FrameworkElement)sender).ReleaseMouseCapture();
            _mousePanning = false;
        }

        private void UserControlMouseLeave(object sender, MouseEventArgs e)
        {
           // _mousePanning = false;
        }

        private void dragDockPanelHost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mousePanning = false;
            e.Handled = true;
        }
    }
}
