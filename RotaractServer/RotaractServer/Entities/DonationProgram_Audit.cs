using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace RotaractServer.Models.DAO_Models
{
    public class DonationProgram_Audit
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DonationProgram_AuditId { get; set; }
            [Required]
            public int DonationProgramId { get; set; }

            [Required]
            public string DonationProgramName { get; set; }
            public string Description { get; set; }
            public string ImageRef { get; set; }
            [Required]
            public DateTime StartDate { get; set; }
            [Required] 
            public DateTime EndDate { get; set; }

            public int Total { get; set; }

            public DateTime Updated_at { get; set; }

            public string Operation { get; set; }




    }
}