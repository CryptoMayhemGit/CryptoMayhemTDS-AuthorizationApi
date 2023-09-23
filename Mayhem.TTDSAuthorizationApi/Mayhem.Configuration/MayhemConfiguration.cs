namespace Mayhem.Configuration
{
    public class MayhemConfiguration
    {
        public string SqlConnectionString { get; }
        public string ZealyApiKey { get; }
        public int PurchasePriceMultiplier { get; }

        public MayhemConfiguration(
            string sqlConnectionString, string zealyApiKey, string purchasePriceMultiplier)
        {
            this.PurchasePriceMultiplier = ToInt(purchasePriceMultiplier);
            this.SqlConnectionString = sqlConnectionString;
            this.ZealyApiKey = zealyApiKey;
        }

        private int ToInt(string purchasePriceMultiplier)
        {
            return Int32.Parse(purchasePriceMultiplier);
        }
    }
}