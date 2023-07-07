using Mayhem.Dal.Dto.Response;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IZealyService
    {
        Task<GetLevelResponse> GetLevelAsync(string wallet);
    }
}
