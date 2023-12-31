﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebMvcProject.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(30)]
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Surname { get; set; }
        [StringLength(50)]
        public string? UserName { get; set; }

        public string? Email { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }
        public bool Locked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";
    }
}
