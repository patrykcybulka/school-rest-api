using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetStudentsByClassAndGroupQueryResult
    {
        public IEnumerable<GetStudentsByClassAndGroupQueryItem> Students { get; set; }
    }
}
