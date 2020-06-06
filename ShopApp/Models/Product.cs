using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string LinkToImage { get; set; }
        public int Count { get; set; }
        public DateTime DateOfPublication { get; set; }

        public string PublisherId { get; set; }
        [JsonIgnore]
        public User Publisher { get; set; }

        public int TypeId { get; set; }
        [JsonIgnore]
        public ProductType Type { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }
}
