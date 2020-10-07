using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotaractServer.Models.DAO_Models;

namespace RotaractServer.Models
{
    public class DonationDto
    {
        public DonationProgram donationProgram { get; set; }
        public int amount { get; set; }
    }
}