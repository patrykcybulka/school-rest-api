using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class GetClassByIdDTO
    {
        [Required]
        public Guid Id { get; set; }
    }
}
