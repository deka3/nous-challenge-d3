using System;
using System.ComponentModel.DataAnnotations;

namespace CleaningManagement.DAL.Models
{
    public class User
    {
        public Guid  Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsActive { get; set; }
    }
}
