using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace RotaractServer.Models
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public int UserId { get; set; }
        public ShippingDetailsDto ShippingDetailsModel { get; set; }
        public string Status { get; set; }
        public string DateStamp { get; set; }
        public double TotalPrice { get; set; }
    }
}