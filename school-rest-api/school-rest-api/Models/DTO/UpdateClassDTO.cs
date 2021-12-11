﻿using System.ComponentModel.DataAnnotations;

namespace school_rest_api.Models.DTO
{
    public class UpdateClassDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Char Name { get; set; }
    }
}
