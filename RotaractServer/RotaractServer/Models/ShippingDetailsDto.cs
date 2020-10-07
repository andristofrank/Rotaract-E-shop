using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class ShippingDetailsDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine { get; set; }
        public string CountyOrRegion { get; set; }
        public int PostalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
    }
}