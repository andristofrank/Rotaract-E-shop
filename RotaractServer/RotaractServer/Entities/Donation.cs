using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class Donation
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DonationId { get; set; }
        public int UserId { get; set; }
        [Required] 
        public int DonationProgram_AuditId { get; set; }
        [Required] 
        public int Amount { get; set; }

    }
}