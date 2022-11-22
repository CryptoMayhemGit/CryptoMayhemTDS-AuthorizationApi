using Mayhem.Util.Classes;

namespace Mayhem.Util.Exceptions
{
    public class NotFoundException : Exception
    {
        public ValidationMessage ValidationMessage { get; set; }

        public NotFoundException(ValidationMessage validationMessage)
        {
            ValidationMessage = validationMessage;
        }
    }
}
