/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface ISessionService : IEntityConfiguration
    {
        Task<List<SessionDto>> GetSessionsByDate(DateTime sessionDate);

        Task<List<SessionDto>> GetSessionsRange(DateTime fromDate, DateTime toDate);

        Task<List<SessionTypeDto>> GetSessionTypes();

        Task<int> CreateSession(SessionDto sessionDto);

        Task<bool> CreateSessionUser(SessionUserDto sessionUserDto);

        Task<SessionDto> GetSession(int sessionId);

        Task<bool> UpdateSession(SessionDto sessionDto);

        Task<bool> DeleteSession(int sessionId);

        Task AddUserToSession(int sessionId, int userId);

        Task RemoveUsersFromSession(int sessionId);

    }
}
