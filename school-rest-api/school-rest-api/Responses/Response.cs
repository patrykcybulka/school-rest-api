using school_rest_api.Enums;

namespace school_rest_api.Responses
{
    public class Response
    {
        public object Result { get; }
        public IEnumerable<EErrorCode> Errors { get; }

        public bool IsSuccess => Errors == null;

        public Response()
        {
        }

        public Response(object result)
        {
            Result = result;
        }

        public Response(EErrorCode error)
        {
            Errors = new List<EErrorCode>() { error };
        }

        public Response(IEnumerable<EErrorCode> errors)
        {
            Errors = errors;
        }
    }
}
