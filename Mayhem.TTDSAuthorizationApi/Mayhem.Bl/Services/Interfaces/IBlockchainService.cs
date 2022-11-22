namespace Mayhem.Bl.Services.Interfaces
{
    public interface IBlockchainService
    {
        Task<bool> VerifyWalletWithSignedMessageAsync(string wallet, string messageToSign, string signedMessage);
    }
}
