using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetStudentsSortedByGenderQueryResult
    {
        public IEnumerable<GetStudentsSortedByGenderQueryItem> Students { get; set; }
    }
}
