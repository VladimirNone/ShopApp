using System;
using System.Collections.Generic;

namespace ShopApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nick { get; set; }

        public int AccountDataId { get; set; }
        public AccountData AccountData { get; set; }


        public List<Comment> Comments { get; set; }
        public List<Product> PublishedProducts { get; set; }
        public List<Order> Orders { get; set; }
    }
}