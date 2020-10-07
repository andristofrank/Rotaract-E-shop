using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class DistrictNo
    {
        [Key]
        [Required]
        public int DistrictNumber { get; set; }

    }
}