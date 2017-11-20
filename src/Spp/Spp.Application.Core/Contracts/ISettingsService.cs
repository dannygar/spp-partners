/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface ISettingsService : IEntityConfiguration
    {
        #region Player
        Task<IList<PlayerPositionDto>> GetPlayerPositions();

        Task<int> CreatePlayerPosition(PlayerPositionDto playerPositionDto);

        Task<bool> UpdatePlayerPosition(PlayerPositionDto playerPositionDto);

        Task<bool> DeletePlayerPosition(int playerPositionId);
        #endregion

        #region SubPosition
        Task<IList<SubPositionDto>> GetSubPositions();

        Task<int> CreateSubPosition(SubPositionDto subPositionDto);

        Task<bool> UpdateSubPosition(SubPositionDto subPositionDto);

        Task<bool> DeleteSubPosition(int subPositionId);
        #endregion

        #region Mesocycle
        Task<IList<MesocycleDto>> GetMesocycles();

        Task<int> CreateMesocycle(MesocycleDto mesocycleDto);

        Task<bool> UpdateMesocycle(MesocycleDto mesocycleDto);

        Task<bool> DeleteMesocycle(int mesocycleId);
        #endregion

        #region Microcycle
        Task<IList<MicrocycleDto>> GetMicrocycles();

        Task<int> CreateMicrocycle(MicrocycleDto microcycleDto);

        Task<bool> UpdateMicrocycle(MicrocycleDto microcycleDto);

        Task<bool> DeleteMicrocycle(int microcycleId);
        #endregion

        #region Image
        Task<IList<ImageDto>> GetImages();

        Task<int> CreateImage(ImageDto imageDto);

        Task<bool> UpdateImage(ImageDto imageDto);

        Task<bool> DeleteImage(int imageId);
        #endregion
    }
}
