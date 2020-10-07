using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models.DAO_Models
{
    public class Products_audit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Product_AuditId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageRef { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Inventory { get; set; }
        [Required]
        public DateTime Updated_at { get; set; }
        [Required]
        public string Operation { get; set; }
    }
}