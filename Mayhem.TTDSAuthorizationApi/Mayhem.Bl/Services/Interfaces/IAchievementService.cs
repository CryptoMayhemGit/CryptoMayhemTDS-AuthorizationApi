using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IAchievementService
    {
        Task<GetLevelResponse> GetLevelAsync(GetLevelRequest request);
    }
}
