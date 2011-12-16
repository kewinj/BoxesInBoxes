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
    /// <summary>
    /// This calculator is used for getting positions for items arranged on a grid.
    /// It assumes that all origin points will be the upper left (0.00, 0.00), and
    /// that there is no space/margin between grid cells
    /// </summary>
    public class GridPositionCalculator
    {
        #region Members

        public double PanelWidth { get; set; }
        public double PanelHeight { get; set; }
        public int ColumnCount { get; set; }
        public int RowCount { get;  set; }

        public double CellWidth
        { get { return CalculateCellWidth(); } }

        public double CellHeight
        { get { return CalculateCellHeight(); } }

        #endregion

        #region Constructor

        public GridPositionCalculator(double panelWidth, double panelHeight, int columnCount, int rowCount)
        {
            PanelWidth = panelWidth;
            PanelHeight = panelHeight;
            ColumnCount = columnCount;
            RowCount = rowCount;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellColumn">Zero indexed</param>
        /// <param name="cellRow">Zero indexed</param>
        /// <returns></returns>
        public Point GetPosition(int cellColumn, int cellRow)
        {
            double x = cellColumn * CellWidth;
            double y = cellRow * CellHeight;

            return new Point(x, y);
        }

        public Point GetPosition(int index)
        {
            CellPosition cellPos = GetCellFromCellIndex(index);
            return GetPosition(cellPos.Column, cellPos.Row);
        }

        public CellPosition GetCellFromPoint(Point p)
        {
            if (p.X < 0.00D || p.X > PanelWidth ||
                p.Y < 0.00D || p.Y > PanelHeight)
            {
                return null;
            }

            int col = Convert.ToInt32(Math.Floor(p.X / CellWidth));
            int row = Convert.ToInt32(Math.Floor(p.Y / CellHeight));

            return new CellPosition() { Column = col, Row = row };
        }

        public CellPosition GetCellFromCellIndex(int position)
        {
            int cellColumn = 0;
            int cellRow = 0;

            cellRow = Convert.ToInt32(Math.Floor(((double)position / (double)ColumnCount)));
            cellColumn = (position - (cellRow * ColumnCount));

            return new CellPosition() { Column = cellColumn, Row = cellRow };
        }

        public int GetIndexFromCell(CellPosition cell)
        {
            int index = cell.Row * ColumnCount + cell.Column;
            return index;
        }

        #endregion

        #region Private Methods

        private double CalculateCellWidth()
        {
            return PanelWidth / ((double)ColumnCount);
        }

        private double CalculateCellHeight()
        {
            return PanelHeight / ((double)RowCount);
        }

        #endregion
    }
}
