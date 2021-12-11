using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class AddEducatorDTO
    {
        [Required]
        public Guid ClassId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}
