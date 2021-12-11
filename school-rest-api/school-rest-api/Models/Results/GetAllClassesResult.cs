using school_rest_api.Models.Results.Items;

namespace school_rest_api.Models.Results
{
    public class GetAllClassesResult
    {
        public IEnumerable<GetAllClassesQueryItem> Classes { get; set; }
    }
}
