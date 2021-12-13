using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetAllStudentsResult
    {
        public IEnumerable<GetAllStudentsItem> Students { get; set; }
    }
}
