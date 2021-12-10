using school_rest_api.Enums;

namespace school_rest_api.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public List<EErrorCode> Errors { get; set; }
    }
}
