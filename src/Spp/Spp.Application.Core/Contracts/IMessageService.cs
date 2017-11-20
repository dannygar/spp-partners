/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IMessageService : IEntityConfiguration
    {
        Task<List<MessageDto>> GetMessages(int playerId);

        Task<int> CreateMessage(MessageDto messageDto);

        Task<MessageDto> GetMessage(int messageId);

        Task<bool> UpdateMessage(MessageDto messageDto);

        Task<bool> DeleteMessage(int messageId);

        Task<bool> DeletePlayerMessages(int playerId);
    }
}
