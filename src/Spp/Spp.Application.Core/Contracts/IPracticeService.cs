/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IPracticeService : IEntityConfiguration
    {
        Task<int> CreatePractice(PracticeDto practiceDto);

        Task<PracticeDto> GetPractice(int practiceId);

        Task<IList<PracticeDto>> GetAllPractices();

        Task<IList<PracticeDto>> GetAllPracticeDetails();

        Task<IList<PracticeDto>> GetSessionPractices(int sessionId);

        Task<IList<PracticeDrillDto>> GetPracticeDrills(int practiceId);

        Task<IList<PracticeDrillDto>> GetPracticeDrills();

        Task<bool> UpdatePractice(PracticeDto practiceDto);

        Task<bool> DeletePractice(int practiceId);

        Task<bool> DeleteAllSessionPractices(int sessionId);

    }
}
