using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace MyControlLibrary.HumanGridLayout
{
    /// <summary>Packer using a custom algorithm by Markus 'Cygon' Ewald</summary>
    /// <remarks>
    ///   <para>
    ///     Algorithm conceived by Markus Ewald (cygon at nuclex dot org), though
    ///     I'm quite sure I'm not the first one to come up with it :)
    ///   </para>
    ///   <para>
    ///     The algorithm always places rectangles as low as possible in the packing
    ///     area. So, for any new rectangle that is to be added, the packer has to
    ///     determine the X coordinate at which the rectangle can have the lowest
    ///     overall height without intersecting any other rectangles.
    ///   </para>
    ///   <para>
    ///     To quickly discover these locations, the packer uses a sophisticated
    ///     data structure that stores the upper silhouette of the packing area. When
    ///     a new rectangle needs to be added, only the silouette edges need to be
    ///     analyzed to find the position where the rectangle would achieve the lowest
    ///     placement possible in the packing area.
    ///   </para>
    /// </remarks>
    public class CygonRectanglePacker
    {
        /// <summary>Initializes a new rectangle packer</summary>
        /// <param name="packingAreaWidth">Maximum width of the packing area</param>
        /// <param name="packingAreaHeight">Maximum height of the packing area</param>
        public CygonRectanglePacker(double packingAreaWidth, double packingAreaHeight)
        {
            PackingAreaWidth = packingAreaWidth;
            PackingAreaHeight = packingAreaHeight;
            HeightSlices = new List<Point> {new Point(0, 0)};
        }

        /// <summary>Maximum width the packing area is allowed to have</summary>
        public double PackingAreaWidth { get; set; }

        /// <summary>Maximum height the packing area is allowed to have</summary>
        public double PackingAreaHeight { get; set; }

        /// <summary>Stores the height silhouette of the rectangles</summary>
        private List<Point> HeightSlices { get; set; }

        /// <summary>Tries to allocate space for a rectangle in the packing area</summary>
        /// <param name="rectangleWidth">Width of the rectangle to allocate</param>
        /// <param name="rectangleHeight">Height of the rectangle to allocate</param>
        /// <param name="placement">Output parameter receiving the rectangle's placement</param>
        /// <returns>True if space for the rectangle could be allocated</returns>
        public bool TryPack(double rectangleWidth, double rectangleHeight, out Point placement)
        {
            // If the rectangle is larger than the packing area in any dimension,
            // it will never fit!
            if ((rectangleWidth > PackingAreaWidth) || (rectangleHeight > PackingAreaHeight))
            {
                placement = new Point();
                return false;
            }

            // Determine the placement for the new rectangle
            bool fits = TryFindBestPlacement(rectangleWidth, rectangleHeight, out placement);

            // If a place for the rectangle could be found, update the height slice table to
            // mark the region of the rectangle as being taken.
            if (fits)
            {
                IntegrateRectangle(placement.X, rectangleWidth, placement.Y + rectangleHeight);
            }

            return fits;
        }

        /// <summary>Finds the best position for a rectangle of the given dimensions</summary>
        /// <param name="rectangleWidth">Width of the rectangle to find a position for</param>
        /// <param name="rectangleHeight">Height of the rectangle to find a position for</param>
        /// <param name="placement">Receives the best placement found for the rectangle</param>
        /// <returns>True if a valid placement for the rectangle could be found</returns>
        private bool TryFindBestPlacement(double rectangleWidth, double rectangleHeight, out Point placement)
        {
            int bestSliceIndex = -1; // Slice index where the best placement was found
            double bestSliceY = 0; // Y position of the best placement found
            double bestScore = PackingAreaHeight; // lower == better!

            // This is the counter for the currently checked position. The search works by
            // skipping from slice to slice, determining the suitability of the location for the
            // placement of the rectangle.
            int leftSliceIndex = 0;

            // Determine the slice in which the right end of the rectangle is located when
            // the rectangle is placed at the far left of the packing area.
            int rightSliceIndex = HeightSlices.BinarySearch(new Point(rectangleWidth, 0), SliceStartComparer.Default);
            if (rightSliceIndex < 0)
            {
                rightSliceIndex = ~rightSliceIndex;
            }

            while (rightSliceIndex <= HeightSlices.Count)
            {
                // Determine the highest slice within the slices covered by the rectangle at
                // its current placement. We cannot put the rectangle any lower than this without
                // overlapping the other rectangles.
                double highest = HeightSlices[leftSliceIndex].Y;
                for (int index = leftSliceIndex + 1; index < rightSliceIndex; ++index)
                {
                    if (HeightSlices[index].Y > highest)
                    {
                        highest = HeightSlices[index].Y;
                    }
                }

                // Only process this position if it doesn't leave the packing area
                if ((highest + rectangleHeight <= PackingAreaHeight))
                {
                    double score = highest;

                    if (score < bestScore)
                    {
                        bestSliceIndex = leftSliceIndex;
                        bestSliceY = highest;
                        bestScore = score;
                    }
                }

                // Advance the starting slice to the next slice start
                ++leftSliceIndex;
                if (leftSliceIndex >= HeightSlices.Count)
                {
                    break;
                }

                // Advance the ending slice until we're on the proper slice again, given the new
                // starting position of the rectangle.
                double rightRectangleEnd = HeightSlices[leftSliceIndex].X + rectangleWidth;
                for (; rightSliceIndex <= HeightSlices.Count; ++rightSliceIndex)
                {
                    double rightSliceStart;
                    if (rightSliceIndex == HeightSlices.Count)
                    {
                        rightSliceStart = PackingAreaWidth;
                    }
                    else
                    {
                        rightSliceStart = HeightSlices[rightSliceIndex].X;
                    }

                    // Is this the slice we're looking for?
                    if (rightSliceStart > rightRectangleEnd)
                        break;
                }

                // If we crossed the end of the slice array, the rectangle's right end has left
                // the packing area, and thus, our search ends.
                if (rightSliceIndex > HeightSlices.Count)
                {
                    break;
                }
            } // while rightSliceIndex <= this.heightSlices.Count

            // Return the best placement we found for this rectangle. If the rectangle
            // didn't fit anywhere, the slice index will still have its initialization value
            // of -1 and we can report that no placement could be found.
            if (bestSliceIndex == -1)
            {
                placement = new Point();
                return false;
            }
            placement = new Point(HeightSlices[bestSliceIndex].X, bestSliceY);
            return true;
        }

        /// <summary>Integrates a new rectangle into the height slice table</summary>
        /// <param name="left">Position of the rectangle's left side</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="bottom">Position of the rectangle's lower side</param>
        private void IntegrateRectangle(double left, double width, double bottom)
        {
            // Find the first slice that is touched by the rectangle
            int startSlice = HeightSlices.BinarySearch(new Point(left, 0), SliceStartComparer.Default);

            // Since the placement algorithm always places rectangles on the slices,
            // the binary search should never some up with a miss!
            Debug.Assert(startSlice >= 0, "Slice starts within another slice");

            // We scored a direct hit, so we can replace the slice we have hit
            double firstSliceOriginalHeight = HeightSlices[startSlice].Y;
            HeightSlices[startSlice] = new Point(left, bottom);

            double right = left + width;
            ++startSlice;

            // Special case, the rectangle started on the last slice, so we cannot
            // use the start slice + 1 for the binary search and the possibly already
            // modified start slice height now only remains in our temporary
            // firstSliceOriginalHeight variable
            if (startSlice >= HeightSlices.Count)
            {
                // If the slice ends within the last slice (usual case, unless it has the
                // exact same width the packing area has), add another slice to return to
                // the original height at the end of the rectangle.
                if (right < PackingAreaWidth)
                {
                    HeightSlices.Add(new Point(right, firstSliceOriginalHeight));
                }
            }
            else
            {
                // The rectangle doesn't start on the last slice
                int endSlice = HeightSlices.BinarySearch(startSlice, HeightSlices.Count - startSlice, new Point(right, 0), SliceStartComparer.Default);

                // Another direct hit on the final slice's end?
                if (endSlice > 0)
                {
                    HeightSlices.RemoveRange(startSlice, endSlice - startSlice);
                }
                else
                {
                    // No direct hit, rectangle ends inside another slice

                    // Make index from negative BinarySearch() result
                    endSlice = ~endSlice;

                    // Find out to which height we need to return at the right end of
                    // the rectangle
                    double returnHeight;
                    if (endSlice == startSlice)
                    {
                        returnHeight = firstSliceOriginalHeight;
                    }
                    else
                    {
                        returnHeight = HeightSlices[endSlice - 1].Y;
                    }

                    // Remove all slices covered by the rectangle and begin a new slice at its end
                    // to return back to the height of the slice on which the rectangle ends.
                    HeightSlices.RemoveRange(startSlice, endSlice - startSlice);
                    if (right < PackingAreaWidth)
                    {
                        HeightSlices.Insert(startSlice, new Point(right, returnHeight));
                    }
                } // if endSlice > 0
            } // if startSlice >= this.heightSlices.Count
        }
    }
}
