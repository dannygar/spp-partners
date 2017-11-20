/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Spp.Data
{
    public sealed class SppDbContext : BaseDbContext
    {
        public IDbConnection SppDbConnection { get; }

        public SppDbContext(DbContextOptions<SppDbContext> options) : base(options)
        {
            SppDbConnection = this.Database.GetDbConnection();
        }
    }
}

