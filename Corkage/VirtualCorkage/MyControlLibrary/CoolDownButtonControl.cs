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

namespace MyControlLibrary
{
    [TemplatePart(Name = PartCore, Type = typeof(Rectangle))]
    [TemplateVisualState(Name = StatePressed, GroupName = NormalStates)]
    [TemplateVisualState(Name = StateMouseOver, GroupName = NormalStates)]
    [TemplateVisualState(Name = StateNormal, GroupName = NormalStates)]
    [TemplateVisualState(Name = StateAvailable, GroupName = CoolDownStates)]
    [TemplateVisualState(Name = StateCoolDown, GroupName = CoolDownStates)]
    public class CoolDownButtonControl : Control
    {
        private FrameworkElement _corePart;
        private bool _isPressed, _isMouseOver, _isCoolDown;
        private DateTime _pressedTime;

        private const string NormalStates = "NormalStates";
        private const string StatePressed = "Pressed";
        private const string StateMouseOver = "MouseOver";
        private const string StateNormal = "Normal";

        private const string CoolDownStates = "CoolDownStates";
        private const string StateAvailable = "Available";
        private const string StateCoolDown = "CoolDown";
        private const string PartCore = "corePart";

        public CoolDownButtonControl()
        {
            this.DefaultStyleKey = typeof(CoolDownButtonControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CorePart = (FrameworkElement)GetTemplateChild(PartCore);
            GoToState(false);
        }

        public FrameworkElement CorePart
        {
            get { return _corePart; }
            set
            {
                FrameworkElement oldCorePart = CorePart;
                if (oldCorePart != null)
                {
                    oldCorePart.MouseEnter -= new MouseEventHandler(_corePart_MouseEnter);
                    oldCorePart.MouseLeave -= new MouseEventHandler(_corePart_MouseLeave);
                    oldCorePart.MouseLeftButtonDown -= new MouseButtonEventHandler(_corePart_MouseLeftButtonDown);
                    oldCorePart.MouseLeftButtonUp -= new MouseButtonEventHandler(_corePart_MouseLeftButtonUp);
                }

                _corePart = value;

                if (CorePart != null)
                {
                    _corePart.MouseEnter += new MouseEventHandler(_corePart_MouseEnter);
                    _corePart.MouseLeave += new MouseEventHandler(_corePart_MouseLeave);
                    _corePart.MouseLeftButtonDown += new MouseButtonEventHandler(_corePart_MouseLeftButtonDown);
                    _corePart.MouseLeftButtonUp += new MouseButtonEventHandler(_corePart_MouseLeftButtonUp);
                }
            }
        }

        void _corePart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).ReleaseMouseCapture();
            if (!CheckCoolDown())
            {
                _isPressed = false;
                _isCoolDown = true;
                _pressedTime = DateTime.Now;
                GoToState(true);
            }
            base.OnMouseLeftButtonUp(e);
        }

        void _corePart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((FrameworkElement)sender).CaptureMouse();
            if (!CheckCoolDown())
            {
                _isPressed = true;
                GoToState(true);
            }
             this.CaptureMouse();
            base.OnMouseLeftButtonDown(e);
        }

        void _corePart_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!CheckCoolDown())
            {
                _isMouseOver = false;
                GoToState(true);
            }
            base.OnMouseLeave(e);
        }

        void _corePart_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!CheckCoolDown())
            {
                _isMouseOver = true;
                GoToState(true);
            }
            base.OnMouseEnter(e);
        }


        private bool CheckCoolDown()
        {
            if (!_isCoolDown)
            {
                return false;
            }
            else
            {
                if (DateTime.Now > _pressedTime.AddSeconds(CoolDownSeconds))
                {
                    _isCoolDown = false;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

     

        

        #region CoolDownSeconds Dependency Property
		public static readonly DependencyProperty CoolDownSecondsProperty =
            DependencyProperty.Register("CoolDownSeconds", typeof(int), typeof(CoolDownButtonControl), 
            new PropertyMetadata(new PropertyChangedCallback(CoolDownButtonControl.OnCoolDownSecondsPropertyChanged)));

        public int CoolDownSeconds
        {
            get { return (int)GetValue(CoolDownSecondsProperty); }
            set { SetValue(CoolDownSecondsProperty, value); }
        }

        private static void OnCoolDownSecondsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoolDownButtonControl cdButton = d as CoolDownButtonControl;
            cdButton.OnCoolDownButtonChange(null);
        } 
	    #endregion

        #region ButtonText Dependency Property
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(CoolDownButtonControl), 
            new PropertyMetadata(new PropertyChangedCallback(CoolDownButtonControl.OnButtonTextPropertyChanged)));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        private static void OnButtonTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoolDownButtonControl cdButton = d as CoolDownButtonControl;
            cdButton.OnCoolDownButtonChange(null);
        }
        #endregion

        protected virtual void OnCoolDownButtonChange(RoutedEventArgs e)
        {
            GoToState(true);
        }

        private void GoToState(bool useTransitions)
        {
            if (_isPressed)
            {
                VisualStateManager.GoToState(this, StatePressed, useTransitions); 
            }
            else if (_isMouseOver)
            {
                VisualStateManager.GoToState(this, StateMouseOver, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, StateNormal, useTransitions);
            }

            if (_isCoolDown)
            {
                VisualStateManager.GoToState(this, StateCoolDown, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, StateAvailable, useTransitions);
            }
        }
    }
}
