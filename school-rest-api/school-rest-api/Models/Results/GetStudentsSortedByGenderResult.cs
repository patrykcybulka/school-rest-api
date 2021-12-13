using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetStudentsSortedByGenderResult
    {
        public IEnumerable<GetStudentsSortedByGenderItem> Students { get; set; }
    }
}
