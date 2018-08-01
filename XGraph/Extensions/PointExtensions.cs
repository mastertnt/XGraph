using System;
using System.Collections.Generic;
using System.Windows;

namespace XGraph.Extensions
{
    /// <summary>
    /// Class extending the <see cref="Point"/> class.
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// Computes the shortest path from start to end.
        /// </summary>
        /// <param name="pStart">The start point.</param>
        /// <param name="pEnd">The end point.</param>
        /// <param name="pPadding">The used padding.</param>
        /// <returns>The path as list of points.</returns>
        public static List<Point> GetShortestPath(this Point pStart, Point pEnd, double pPadding)
        {
            List<Point> lResult = new List<Point>();

            // First, look for the max between with and height.
            Double lHeight = Math.Abs(pEnd.Y - pStart.Y);
            Double lWidth = Math.Abs(pEnd.X - pStart.X);


            if
                (lHeight < 100)
            {
                if (pStart.X > pEnd.X)
                {
                    lResult.Add(pStart);
                    lResult.Add(new Point(pStart.X + pPadding, pStart.Y - (pPadding / 2)));
                    lResult.Add(new Point(pEnd.X - pPadding, pStart.Y - (pPadding / 2)));
                    lResult.Add(pEnd);
                }
                else
                {
                    Double lNewX = pStart.X + lWidth / 2.0;
                    lResult.Add(pStart);
                    lResult.Add(new Point(lNewX, pStart.Y));
                    lResult.Add(new Point(lNewX, pEnd.Y));
                    lResult.Add(pEnd);
                }
            }
            else
            {
                lResult.Add(pStart);
                lResult.Add(new Point(pStart.X + pPadding, pStart.Y));
                lResult.Add(new Point(pEnd.X - pPadding, pEnd.Y));
                lResult.Add(pEnd);
            }

            

            return lResult;
        }
    }
}

