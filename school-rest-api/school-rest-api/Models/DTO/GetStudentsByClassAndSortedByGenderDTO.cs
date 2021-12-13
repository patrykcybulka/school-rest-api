using school_rest_api.Enums;
using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class GetStudentsByClassAndSortedByGenderDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public EGender Gender { get; set; }
    }
}
