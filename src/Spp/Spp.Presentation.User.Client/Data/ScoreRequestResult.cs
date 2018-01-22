/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/

namespace Spp.Presentation.User.Client.Data
{
    public class Value
    {
        public string[] ColumnNames { get; set; }
        public string[] ColumnTypes { get; set; }
        public string[,] Values { get; set; }
    }

    public class Output1
    {
        public string Type { get; set; }
        public Value Value { get; set; }
    }

    public class Results
    {
        public Output1 Output1 { get; set; }
    }

    public class ScoreRequestResult
    {
        public Results Results { get; set; }
    }
}
