namespace school_rest_api.Entries
{
    public class EducatorEntry
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
