namespace Mayhem.Util.Classes
{
    public class ErrorModel
    {
        public string Code { get; }

        public string Message { get; }

        public ErrorModel(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
