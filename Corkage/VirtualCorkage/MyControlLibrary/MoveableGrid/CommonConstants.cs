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
    public class CommonConstants
    {
        public const int GRID_COLUMNS = 4;
        public const int GRID_ROWS = 4;

        public const int INITIAL_ICON_COUNT = GRID_COLUMNS * GRID_ROWS - 1;

        public static TimeSpan CANVAS_ICON_MOVING_TIMING = new TimeSpan(0, 0, 0, 0, 150);
    }
}
