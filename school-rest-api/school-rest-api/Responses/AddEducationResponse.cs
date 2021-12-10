namespace school_rest_api.Responses
{
    public class AddEducationResponse : BaseResponse
    {
        public Guid Id { get; }

        public AddEducationResponse(Guid id)
        {
            Id = id
        }
    }
}
