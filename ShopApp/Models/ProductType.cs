using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string NameOfType { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
    }
}
