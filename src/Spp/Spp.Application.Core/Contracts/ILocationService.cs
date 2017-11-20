/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface ILocationService : IEntityConfiguration
    {
        Task<List<LocationDto>> GetLocations();

        Task<int> CreateLocation(LocationDto locationDto);

        Task<bool> UpdateLocatioin(LocationDto locationDto);

        Task<bool> DeleteLocation(int locationId);

    }
}
