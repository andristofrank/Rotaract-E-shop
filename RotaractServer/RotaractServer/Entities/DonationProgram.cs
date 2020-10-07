using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{ 
        /// <summary>
        /// The main <c>Donation</c> class.
        ///  Contains all methods for getting and setting all Donation's Entities
        /// </summary>
        public class DonationProgram
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DonationProgramId { get; set; }

            [Required] public string DonationProgramName { get; set; }
            public string Description { get; set; }
            public string ImageRef { get; set; }
            [Required]  public DateTime StartDate { get; set; }
            [Required] public DateTime EndDate { get; set; }
            public int Total { get; set; }

    }
}