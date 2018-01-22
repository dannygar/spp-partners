/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Helpers
{
    using System;
    using Windows.Foundation;
    using Windows.UI.Xaml.Shapes;

    /// <summary>Helper class for performing actions on geometric plane </summary>
    public static class Geometry
    {
        /// <summary>Get point on line moved from the line end on percent from length value. Useful for scaling. </summary>
        /// <param name="line">The line.</param>
        /// <param name="percentage">The offset.</param>
        /// <returns>Desired point.</returns>
        public static Point LinePointPercentageOffset(Line line, double percentage)
        {
            var start = new Point { X = line.X1, Y = line.Y1 };
            var end = new Point { X = line.X2, Y = line.Y2 };

            var result = new Point
            {
                X = start.X + (end.X - start.X) * percentage / 100,
                Y = start.Y + (end.Y - start.Y) * percentage / 100,
            };

            return result;
        }

        /// <summary>Get point on line moved from the line end on exact points offset. </summary>
        /// <param name="line">The line.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Desired point.</returns>
        public static Point LinePointPixelOffset(Line line, double offset)
        {
            var start = new Point { X = line.X1, Y = line.Y1 };
            var end = new Point { X = line.X2, Y = line.Y2 };

            var length = Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
            var newLength = length + offset;

            var norma = new Point
            {
                X = (end.X - start.X) / length,
                Y = (end.Y - start.Y) / length,
            };

            var result = new Point
            {
                X = start.X + norma.X * newLength,
                Y = start.Y + norma.Y * newLength,
            };

            return result;
        }

        /// <summary>Rotates point around the specified pivot. </summary>
        /// <param name="pivot">The pivot.</param>
        /// <param name="p">The rotated point.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        /// <returns>Resulting point.</returns>
        public static Point Rotate(Point pivot, Point p, double angle)
        {
            p.X -= pivot.X;
            p.Y -= pivot.Y;

            var radians = angle * Math.PI / 180;

            return new Point
            {
                X = p.X * Math.Cos(radians) - p.Y * Math.Sin(radians) + pivot.X,
                Y = p.X * Math.Sin(radians) + p.Y * Math.Cos(radians) + pivot.Y,
            };
        }
    }
}
