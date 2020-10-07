using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageRef { get; set; }
        [Required]
        public double Price { get; set; }
        public int Inventory { get; set; }
    }
}