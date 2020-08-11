using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderState State { get; set; }
        public string FinalLocation { get; set; }
        public string ReasonForCancellation { get; set; }

        public DateTime DateOfOrdering { get; set; }
        public DateTime DateOfPaing { get; set; }
        public DateTime DateOfClosing { get; set; }

        public string CustomerId { get; set; }
        [JsonIgnore]
        public User Customer { get; set; }
        
        
        public List<OrderedProduct> OrderedProducts { get; set; }
    }
}
