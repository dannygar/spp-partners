/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Presentation.User.Client.Data;

namespace Spp.Presentation.User.Client.Helpers
{
    public class PlayerHelper
    {
        public static bool IsResting(Athlete player)
        {
            return player.IsResting.HasValue && player.IsResting.Value;
        }

        public static int ComparePlayers(Athlete first, Athlete second)
        {
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
