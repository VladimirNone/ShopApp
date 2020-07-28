using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeOfBuing { get; set; }
        public int Count { get; set; }

        public string CustomerId { get; set; }
        public User Customer { get; set; }

        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
