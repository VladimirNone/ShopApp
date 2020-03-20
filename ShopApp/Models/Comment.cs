using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime TimePublished { get; set; }
        public string Body { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int? AuthorId { get; set; }
        public User Author { get; set; }
    }
}
