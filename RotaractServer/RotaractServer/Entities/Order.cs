using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class Order
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public string Status { get; set; }
        [Required] public DateTime DateStamp { get; set; }
        [Required] public double TotalPrice { get; set; }

    }
}