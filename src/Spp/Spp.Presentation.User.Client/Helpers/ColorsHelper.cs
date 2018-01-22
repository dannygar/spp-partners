/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Helpers
{
    using System;
    using System.Linq;
    using Windows.UI;

    public static class ColorsHelper
    {
        public static Color FromRgbString(string color)
        {
            Guard.ThrowIfNull(color, nameof(color));

            var c = color.Trim('#');
            byte cl;
            switch (c.Length)
            {
                case 3: cl = 1; break;
                case 6: case 8: cl = 2; break;
                default: throw new ArgumentException($"Incorrect RGB color format for parameter \"{nameof(color)}\".");
            }

            var cc = GetComponents(c, cl);

            return ColorHelper.FromArgb((byte)cc.A, (byte)cc.R, (byte)cc.G, (byte)cc.B);
        }

        private static dynamic GetComponents(string s, byte cl)
        {
            var chunks = Enumerable
                .Range(0, s.Length / cl)
                .Select(i => Convert.ToByte(s.Substring(i * cl, cl), 16))
                .ToArray();

            var startIndex = chunks.Length == 4 ? 1 : 0;
            var alpha = chunks.Length == 4 ? chunks[0] : 255;

            return new
            {
                R = chunks[startIndex],
                G = chunks[startIndex + 1],
                B = chunks[startIndex + 2],
                A = alpha,
            };
        }
    }
}
