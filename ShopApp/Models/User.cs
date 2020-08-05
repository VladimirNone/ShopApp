using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopApp.Models
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public DateTime DateOfRegistration { get; set; }
        [JsonIgnore]
        public bool UserDeleted { get; set; }

        [JsonIgnore]
        public List<Comment> Comments { get; set; }
        [JsonIgnore]
        public List<Product> PublishedProducts { get; set; }
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }
}