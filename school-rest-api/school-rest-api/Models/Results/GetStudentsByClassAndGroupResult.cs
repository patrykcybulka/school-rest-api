using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetStudentsByClassAndGroupResult
    {
        public IEnumerable<GetStudentsByClassAndGroupItem> Students { get; set; }
    }
}
