using school_rest_api.Enums;

namespace school_rest_api.Exceptions
{
    public static class Guard
    {
        public static void IsTrue(bool condition, EErrorCode errorCode)
        {
            if (condition)
                throw new SchoolException(errorCode);
        }
    }
}
