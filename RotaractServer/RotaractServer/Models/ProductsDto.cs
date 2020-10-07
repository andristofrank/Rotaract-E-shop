using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotaractServer.Models
{
    public class ProductsDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageRef { get; set; }
        public double Price { get; set; }
        public int Inventory { get; set; }
    }
}