using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class ShippingDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShippingDetailsId { get; set; }

        [Required] public int OrderId { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        [Required] public string AddressLine { get; set; }
        [Required] public string CountyOrRegion { get; set; }
        [Required] public int PostalCode { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public int PhoneNumber { get; set; }
    }
}