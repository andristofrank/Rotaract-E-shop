using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class DonationProgramDto
    {
        public int DonationProgramId { get; set; }

        public string DonationProgramName { get; set; }
        public string Description { get; set; }
        public string ImageRef { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Total { get; set; }
    }
}