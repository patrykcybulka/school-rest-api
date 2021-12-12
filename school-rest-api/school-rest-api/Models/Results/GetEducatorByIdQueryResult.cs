namespace school_rest_api.Models.Results
{
    public class GetEducatorByIdQueryResult
    {
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
