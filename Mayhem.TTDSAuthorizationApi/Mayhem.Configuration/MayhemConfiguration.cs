namespace Mayhem.Configuration
{
    public class MayhemConfiguration
    {
        public string SqlConnectionString { get; }

        public MayhemConfiguration(
            string sqlConnectionString)
        {
            SqlConnectionString = sqlConnectionString;
        }
    }
}