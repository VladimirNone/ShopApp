using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ShopApp.Models
{
    public class User : IdentityUser
    {
        public DateTime DateOfRegistration { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Product> PublishedProducts { get; set; }
        public List<Order> Orders { get; set; }
    }
}