using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetAllEducatorsQueryResult
    {
        public IEnumerable<GetAllEducatorsQueryItem> Educators { get; set; }
    }
}
