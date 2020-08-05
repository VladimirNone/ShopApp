 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public DateTime TimeOfBuing { get; set; }
        public int Count { get; set; }

        public bool Cancelled { get; set; } 
        public string CurrentLocation { get; set; }
        public string ReasonForCancellation { get; set; }

        public int ProductId { get; set; }
        
        public Product Product { get; set; }

        public int? OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
    }
}
