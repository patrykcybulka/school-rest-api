using school_rest_api.Entries;
using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class AddStudentDTO
    {
        [Required]
        public Guid ClassId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public EGender Gender { get; set; }

        [Required]
        public ELanguageGroup LanguageGroup { get; set; }
    }
}
