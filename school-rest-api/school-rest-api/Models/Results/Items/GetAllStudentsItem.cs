using school_rest_api.Enums;

namespace school_rest_api.Models.Results.Items
{
    public class GetAllStudentsItem
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EGender Gender { get; set; }
        public ELanguageGroup LanguageGroup { get; set; }
    }
}
