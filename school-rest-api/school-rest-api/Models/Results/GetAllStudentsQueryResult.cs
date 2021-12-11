using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetAllStudentsQueryResult
    {
        public IEnumerable<GetAllStudentsQueryItem> Students { get; set; }
    }
}
