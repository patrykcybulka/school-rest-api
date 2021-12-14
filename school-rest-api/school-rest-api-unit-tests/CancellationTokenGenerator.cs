using System.Threading;

namespace school_rest_api_unit_tests
{
    public sealed class CancellationTokenGenerator
    {
        public static CancellationToken Generate()
        {
            return new CancellationTokenSource().Token;
        }
    }
}
