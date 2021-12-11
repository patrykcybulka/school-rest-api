using school_rest_api.Entries;

namespace school_rest_api.Models.Results.Items
{
    public class GetAllStudentsQueryItem
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EGender Gender { get; set; }
        public ELanguageGroup LanguageGroup { get; set; }
    }
}
