using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotaractServer.Models.DAO_Models;

namespace RotaractServer.Models
{
    public class OrderItemDto
    {
        public Product product { get; set; }
        public int Quantity { get; set; }
    }
}