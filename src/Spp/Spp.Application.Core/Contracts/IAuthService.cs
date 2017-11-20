/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IAuthService : IEntityConfiguration
    {
        Task<int> AuthenticateUser(string userName);

        Task<int> CreateUser(UserDto userDto);

        Task<UserDto> GetUser(int userId);

        Task<bool> UpdateUser(UserDto userDto);

        Task<int> DeleteUser(int userId);

        //Task<AppCredentialsDTO> GetToken(AppCredentialsDTO appCredentials);
    }
}
