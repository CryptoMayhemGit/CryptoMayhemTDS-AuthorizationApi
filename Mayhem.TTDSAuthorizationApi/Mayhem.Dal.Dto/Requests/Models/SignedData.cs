namespace Mayhem.Dal.Dto.Requests.Models
{
    public class SignedData
    {
        public string Wallet { get; set; }
        public string Nonce { get; set; }
        public string Message { get; set; }
        public string Handle { get; set; }
    }
}
