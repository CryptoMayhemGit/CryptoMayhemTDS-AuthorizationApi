namespace Mayhem.Configuration
{
    public class MayhemConfiguration
    {
        public string SqlConnectionString { get; }
        public string ZealyApiKey { get; }

        public MayhemConfiguration(
            string sqlConnectionString, string zealyApiKey)
        {
            SqlConnectionString = sqlConnectionString;
            ZealyApiKey = zealyApiKey;
        }
    }
}