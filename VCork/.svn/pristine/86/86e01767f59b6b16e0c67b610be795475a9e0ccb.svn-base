using System.Collections.Generic;
using System.Windows;

namespace MyControlLibrary.HumanGridLayout
{
    /// <summary>Compares the starting position of height slices</summary>
    public class SliceStartComparer : IComparer<Point>
    {
        /// <summary>Provides a default instance for the anchor rank comparer</summary>
        public static readonly SliceStartComparer Default = new SliceStartComparer();

        /// <summary>Compares the starting position of two height slices</summary>
        /// <param name="left">Left slice start that will be compared</param>
        /// <param name="right">Right slice start that will be compared</param>
        /// <returns>The relation of the two slice starts ranks to each other</returns>
        public int Compare(Point left, Point right)
        {
            return (int)(left.X - right.X);
        }
    }
}