using System.Collections.Generic;
using System.Threading.Tasks;
using Spp.Application.Core.Models;

namespace Spp.Application.Core.Contracts
{
    public interface IWellnessService
    {
        Task<List<WellnessDto>> GetWellnessesByPlayerId(int playerId);


        Task<bool> SubmitWellness(WellnessDto dto);
    }
}
