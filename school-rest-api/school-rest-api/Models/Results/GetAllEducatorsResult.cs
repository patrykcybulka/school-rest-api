using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetAllEducatorsResult
    {
        public IEnumerable<GetAllEducatorsItem> Educators { get; set; }
    }
}
