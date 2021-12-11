using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class AddClassDTO
    {
        [Required]
        public Char Name { get; set; }
    }
}
