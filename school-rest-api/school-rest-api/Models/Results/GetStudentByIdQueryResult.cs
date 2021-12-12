using school_rest_api.Enums;

namespace school_rest_api.Models.Results
{
    public class GetStudentByIdQueryResult
    {
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EGender Gender { get; set; }
        public ELanguageGroup LanguageGroup { get; set; }
    }
}
