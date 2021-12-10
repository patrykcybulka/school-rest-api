using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace school_rest_api.Entries
{
    public class StudentEntry
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EGender Gender { get; set; }
        public ELanguageGroup LanguageGroup { get; set; }
    }
}
