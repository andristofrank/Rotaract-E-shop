using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class OrderItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Required] public int OrderId { get; set; }
        [Required] public int Product_AuditId { get; set; }
        [Required] public int Quantity { get; set; }

    }
}