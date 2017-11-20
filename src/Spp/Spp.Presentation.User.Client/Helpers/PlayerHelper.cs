/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Helpers
{
    using System;
    using Data;

    public class PlayerHelper
    {
        public static bool IsResting(Athlete player)
        {
            return player.IsResting.HasValue && player.IsResting.Value;
        }

        public static int ComparePlayers(Athlete first, Athlete second)
        {
            /*
            // TODO: Replace by categories after 15th November.
            if (first.Depth != null || second.Depth != null)
            {
                if (first.Depth == null || first.Depth > second.Depth)
                {
                    return 1;
                }

                if (second.Depth == null || first.Depth < second.Depth)
                {
                    return -1;
                }
            }
            */

            if (IsResting(first) && !IsResting(second))
            {
                return 1;
            }

            if (!IsResting(first) && IsResting(second))
            {
                return -1;
            }

            return string.Compare(first.LastName, second.LastName, StringComparison.CurrentCulture);
        }
    }
}
