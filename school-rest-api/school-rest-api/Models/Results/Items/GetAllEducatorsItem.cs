namespace school_rest_api.Models.Results.Items
{
    public class GetAllEducatorsItem
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
