using school_rest_api.Enums;

namespace school_rest_api.Exceptions
{
    public class SchoolException : Exception
    {
        public EErrorCode ErrorCode { get; set; }

        public SchoolException(EErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
