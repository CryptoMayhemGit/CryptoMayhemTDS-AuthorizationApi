namespace Mayhem.Bl.Services.Interfaces
{
    public interface IVestingService
    {
        Task<int> GetBlockedTokens(string walletAddress);
    }
}
