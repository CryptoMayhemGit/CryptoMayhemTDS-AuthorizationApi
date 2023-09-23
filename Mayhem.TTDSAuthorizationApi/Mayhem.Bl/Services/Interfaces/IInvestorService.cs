using Mayhem.Dal.Dto.Response;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IInvestorService
    {
        Task<GetInvestorStatusResponse?> CheckIsExistAsync(string wallet);
    }
}
