namespace Mayhem.Dal.Dto.Response
{
    /// <summary>
    /// Cyber Connect Login Response
    /// </summary>
    public class CcLoginResponse
    {
        public CcData data { get; set; }
    }

    /// <summary>
    /// Cyber Connect Data
    /// </summary>
    public class CcData
    {
        public CcLoginVerify loginVerify { get; set; }
    }

    /// <summary>
    /// Cyber Connect Login Verify
    /// </summary>
    public class CcLoginVerify
    {
        public string accessToken { get; set; }
    }
}
