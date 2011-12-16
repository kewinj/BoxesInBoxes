using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RearrangableGridCanvas : Canvas
    {
        #region Members

        public bool IsEditing { get; set; }

        private GridPositionCalculator GridCalculator;
        private List<IIcon> IconStack_Realtime;
        private List<IIcon> IconStack_LastChanged;
        private List<IIcon> IconStack_LastCommitted;

        private IIcon MouseCapturedIcon { get; set; }
        private Point MouseCapturedClickPosition { get; set; }

        private int RowCount { get; set; }
        private int ColumnCount { get; set; }
        private int CellCount
        {
            get { return RowCount * ColumnCount; }
        }

        public Point IconSize
        {
            get
            {
                return new Point(GridCalculator.CellWidth, GridCalculator.CellHeight);
            }
        }

        #endregion

        #region Public Methods

        public void Initialize(int columnCount, int rowCount)
        {
            IsEditing = false;
            MouseCapturedIcon = null;
            MouseCapturedClickPosition = new Point(0.00D, 0.00D);
            ColumnCount = columnCount;
            RowCount = rowCount;

            //sets background to black
            Background = new SolidColorBrush(Colors.Transparent);

            GridCalculator = new GridPositionCalculator(this.Width, this.Height, ColumnCount, RowCount);

            IconStack_Realtime = new List<IIcon>();
            IconStack_LastChanged = new List<IIcon>();
            IconStack_LastCommitted = new List<IIcon>();

            // make all of our collections be the size of the number of positions
            // based on the row/column count
            PadCollectionWithNull(IconStack_Realtime);
            PadCollectionWithNull(IconStack_LastChanged);
            PadCollectionWithNull(IconStack_LastCommitted);
        }

        public void AddIcon(IIcon icon, int pos)
        {
            if (pos >= CellCount)
            {
                RowCount += 1;
                for (int i = 0; i < ColumnCount; i++)
                {
                    IconStack_Realtime.Add(null);
                    IconStack_LastChanged.Add(null);
                    IconStack_LastCommitted.Add(null);
                }
                
                this.Height += IconSize.Y;
                GridCalculator.PanelHeight += IconSize.Y;

                GridCalculator.RowCount = RowCount;
                GridCalculator.ColumnCount = ColumnCount;
            }

            // TODO:: sanity check that position is within bounds
            // TODO:: sanity check that IIcon is a UIElement           
            UIElement iconUIElement = icon as UIElement;
            iconUIElement.SetValue(Canvas.ZIndexProperty, 0);
            this.Children.Add(iconUIElement);

            IconStack_Realtime[pos] = icon;
            IconStack_LastChanged[pos] = icon;
            IconStack_LastCommitted[pos] = icon;

            int position = GetIconStackPosition(icon);
            Point canvasPosition = GridCalculator.GetPosition(position);
            UIElement iconElement = icon as UIElement;

            // we do an initial render transform so that later when we animate it no exception is thrown
            TransformGroup tg = TransitionHelper.GetTransformGroupForXYOffset(canvasPosition.X, canvasPosition.Y, 1.00D, 1.00D);
            iconElement.RenderTransform = tg;

            iconUIElement.MouseLeftButtonDown += new MouseButtonEventHandler(iconUIElement_MouseLeftButtonDown);
            iconUIElement.MouseLeftButtonUp += new MouseButtonEventHandler(iconUIElement_MouseLeftButtonUp);
            iconUIElement.MouseMove += new MouseEventHandler(iconUIElement_MouseMove);
            iconUIElement.Drop += new DragEventHandler(iconUIElement_Drop);
        }

        void iconUIElement_Drop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void CommitChanges()
        {
            IconStack_LastCommitted = CloneStack(IconStack_LastChanged);
        }

        public void CancelChanges()
        {
            IconStack_LastChanged = CloneStack(IconStack_LastCommitted);
            IconStack_Realtime = CloneStack(IconStack_LastCommitted);

            UpdateLayoutFromRealtimeStack();
        }

        #endregion

        #region Event Handlers

        void iconUIElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsEditing)
                return;

            UIElement element = sender as UIElement;
            element.Opacity = 0.60D;
            element.SetValue(Canvas.ZIndexProperty, 10); // move it to the top so its over other blocks
            element.CaptureMouse();
            MouseCapturedIcon = element as IIcon;            

            Point clickPoint = e.GetPosition(element);
            MouseCapturedClickPosition = new Point(clickPoint.X, clickPoint.Y);
            e.Handled = true;
        }

        void iconUIElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsEditing)
                return;

            // TODO:: use constants for ZIndex
            UIElement element = sender as UIElement;
            TransformGroup tg = TransitionHelper.GetTransformGroupForXYOffset(
                e.GetPosition(this).X - MouseCapturedClickPosition.X,
                e.GetPosition(this).Y - MouseCapturedClickPosition.Y,
                1.0D, 1.0D);
            element.RenderTransform = tg;
            element.Opacity = 1.00D;
            element.SetValue(Canvas.ZIndexProperty, 0); // move the block back down to the common zindex
            element.ReleaseMouseCapture();

            Point mousePosition = e.GetPosition(this);

            // get a handle on the icon the mouse is over
            CellPosition cellPosition = GetCellPositionFromMousePoint(mousePosition);

            // if the cell position is null then the user is out of bounds, assume the icon
            // will go to the last position
            if (cellPosition == null)
            {
                cellPosition = GridCalculator.GetCellFromCellIndex(CellCount - 1);
            }

            // take the Icon which was captured by the mouse 
            // and put it into the Icon stack at the provided position
            UpdateIconStack(ref IconStack_LastChanged, MouseCapturedIcon, cellPosition);

            // the icon is no longer being dragged by the mouse, so clear out the captured icon
            MouseCapturedIcon = null;

            // transition to this stack (and update the realtime stack)
            TransitionToIconStack(IconStack_LastChanged);            
        }

        void iconUIElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsEditing)
                return;

            if (MouseCapturedIcon != null)
            {
                // update the icon being dragged around's position
                UIElement iconElement = MouseCapturedIcon as UIElement;
                TransformGroup tg = TransitionHelper.GetTransformGroupForXYOffset(
                    e.GetPosition(this).X - MouseCapturedClickPosition.X, 
                    e.GetPosition(this).Y - MouseCapturedClickPosition.Y, 
                    1.2D, 1.2D);
                iconElement.RenderTransform = tg;

                Point mousePosition = e.GetPosition(this);

                // get a handle on the icon the mouse is over
                CellPosition cellPosition = GetCellPositionFromMousePoint(mousePosition);

                // if the cell position is null then the user is out of bounds, assume the icon
                // will go to the last position
                if (cellPosition == null)
                {
                    cellPosition = GridCalculator.GetCellFromCellIndex(CellCount - 1);
                }

                // take the Icon which was captured by the mouse 
                // and put it into the Icon stack at the provided position
                UpdateIconStack(ref IconStack_Realtime, MouseCapturedIcon, cellPosition);
                
                // transition to this stack (and update the realtime stack)
                TransitionToIconStack(IconStack_Realtime);
            }
        }

        #endregion

        #region Private Methods

        private void PadCollectionWithNull(List<IIcon> iconList)
        {
            if (iconList.Count > CellCount)
                throw new InvalidProgramException("FillCollectionWithNull() asked to fill up a collection already over the requested size, cannot continue.");

            while (iconList.Count < CellCount)
            {
                iconList.Add(null);
            }
        }

        private IIcon GetIcon(int index)
        {
            if (index < 0 || index >= IconStack_Realtime.Count)
                return null;

            return IconStack_Realtime[index];
        }

        private int GetIconStackPosition(IIcon icon)
        {
            return IconStack_Realtime.IndexOf(icon);
        }

        private CellPosition GetCellPositionFromMousePoint(Point p)
        {
            // detect the icon we are currently over
            CellPosition overCellPosition = GridCalculator.GetCellFromPoint(p);
            return overCellPosition;
        }

        private int GetCellIndexFromMousePoint(Point p)
        {
            CellPosition overCellPosition = GetCellPositionFromMousePoint(p);
            if (overCellPosition != null)
            {
                int overCellIndex = GridCalculator.GetIndexFromCell(overCellPosition);
                return overCellIndex;
            }

            return -1;
        }

        private IIcon GetIconFromMousePoint(Point p)
        {
            int overCellIndex = GetCellIndexFromMousePoint(p);
            if (overCellIndex != -1)
            {
                return GetIcon(overCellIndex);
            }

            return null;
        }

        private List<IIcon> CloneStack(List<IIcon> sourceIconStack)
        {
            List<IIcon> returnStack = (from icon in sourceIconStack
                                       select icon).ToList<IIcon>();
            PadCollectionWithNull(returnStack);
            return returnStack;
        }

        private void TransitionToIconStack(List<IIcon> iconStack)
        {
            // update the realtime stack to match the provided icon stack
            IconStack_Realtime = CloneStack(iconStack);

            // update the layout from the realtime stack
            UpdateLayoutFromRealtimeStack();
        }

        private void UpdateIconStack(ref List<IIcon> iconStack, IIcon mouseCapturedIcon, CellPosition mouseCellPosition)
        {
            // if the cell spot is empty then find where this icon was before, 
            // remove it, and put it into the new position in the stack
            int cellIndex = GridCalculator.GetIndexFromCell(mouseCellPosition);
            if (iconStack[cellIndex] == null)
            {
                int removeIndex = iconStack.IndexOf(mouseCapturedIcon);
                iconStack[removeIndex] = null;
                iconStack[cellIndex] = mouseCapturedIcon;

                ContractAndPadStack(ref iconStack);
            }
            else // .. the cell position isn't empty, so make a blank spot here
            {
                UpdateIconStack_MakeBlankSpotAtIndex(iconStack, mouseCapturedIcon, cellIndex);
                iconStack[cellIndex] = mouseCapturedIcon;

                ContractAndPadStack(ref iconStack);
            }
        }

        private void ContractAndPadStack(ref List<IIcon> iconStack)
        {
            List<IIcon> newIconStack = (from ic in iconStack
                         where ic != null
                         select ic).ToList<IIcon>();
            PadCollectionWithNull(newIconStack);
            iconStack = newIconStack;
        }

        private void UpdateIconStack_MakeBlankSpotAtIndex(List<IIcon> iconStack, IIcon mouseCapturedIcon, int index)
        {
            // we are going to consider a blank position a spot in the icon stack which is null, 
            // or a spot in the stack which is the captured icon itself (since when it is picked up that
            // spot is empty (even if it isnt in the stack array)

            // first make sure that the spot requested isn't already blank, if so then there's nothing to do
            if (IsSpotBlank(iconStack, mouseCapturedIcon, index))
                return;

            // find the closest blank spot
            int closestBlankSpotIndex = FindFirstBlankSpotIndex(iconStack, mouseCapturedIcon, index);

      
            if (closestBlankSpotIndex == iconStack.IndexOf(mouseCapturedIcon))
            {
                iconStack.RemoveAt(closestBlankSpotIndex);
                iconStack.Insert(index, mouseCapturedIcon);
            }
            else
            {
                iconStack.RemoveAt(closestBlankSpotIndex);
                iconStack.Insert(index, null);                    
            }
        }

        private bool IsSpotBlank(List<IIcon> iconStack, IIcon mouseCapturedIcon, int index)
        {
            return (iconStack[index] == null || iconStack[index] == mouseCapturedIcon);
        }

        private int FindFirstBlankSpotIndex(List<IIcon> iconStack, IIcon mouseCapturedIcon, int index)
        {
            // TODO:: optimize this query
            var blankQuery = from icon in iconStack
                             where IsSpotBlank(iconStack, mouseCapturedIcon, iconStack.IndexOf(icon))
                             orderby iconStack.IndexOf(icon) ascending
                             select iconStack.IndexOf(icon);

            if (blankQuery.Count() > 0)
                return blankQuery.First();

            return -1;
        }
        
        private void UpdateLayoutFromRealtimeStack()
        {
            // iterate through all the positions in the icon stack and animate the blocks
            // to move to their specified locations
            for (int index = 0; index < IconStack_Realtime.Count; index++)
            {
                // there could be nothing in this position, if so then skip this index/position
                IIcon icon = IconStack_Realtime[index];
                if (icon == null)
                    continue;

                // if there is an icon currently being moved then we don't want to update it
                if (MouseCapturedIcon != null && index == IconStack_Realtime.IndexOf(MouseCapturedIcon))
                    continue;

                // get a handle on the icon UIElement and where it should go for this index
                UIElement iconElement = icon as UIElement;
                Point iconPosition = GridCalculator.GetPosition(index);

                // build a moving storyboard and start it
                Storyboard sbMove = TransitionHelper.BuildTranslateTransformTransition(iconElement,
                    iconPosition, CommonConstants.CANVAS_ICON_MOVING_TIMING, false);

                sbMove.Begin();
            }
        }

        #endregion
    }
}