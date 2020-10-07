using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Username { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int DistrictNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

    }
}