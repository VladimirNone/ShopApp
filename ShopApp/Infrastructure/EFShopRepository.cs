using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Infrastructure
{
    public class EFShopRepository : IRepository
    {
        private ShopAppDbContext db { get; set; }
        public EFShopRepository(ShopAppDbContext dbContext)
            => db = dbContext;


        public void AddComment(Comment comment)
            => db.Comments.Add(comment);

        public void AddOrder(Order order)
            => db.Orders.Add(order);

        public void AddProduct(Product product)
            => db.Products.Add(product);

        public void AddUser(User user)
            => db.Users.Add(user);

        public List<Product> FindProductsByName(string partOfName)
            => db.Products.Where(p => p.Name.Contains(partOfName)).ToList();

        public List<Comment> GetCommentsFromProduct(Product product)
            => db.Products.Include(h => h.Comments).Single(h => h.Id == product.Id).Comments;

        public List<Comment> GetCommentsFromUser(User userOwner)
            => db.Users.Include(h=>h.Comments).Single(h => h.Id == userOwner.Id).Comments;

        public List<Order> GetOrdersFromUser(User userOwner)
            => db.Users.Include(h => h.Orders).Single(h => h.Id == userOwner.Id).Orders;

        public Product GetProductById(int id)
            => db.Products.Single(p => p.Id == id);

        public List<Product> GetProductsFromUser(User userAuthor)
            => db.Users.Include(h => h.PublishedProducts).Single(h => h.Id == userAuthor.Id).PublishedProducts;

        public List<ProductType> GetProductTypes()
            => db.ProductTypes.ToList();

        public User GetUserById(int id)
            => db.Users.Find(id);

        public List<Product> GetProductsByProductTypeName(string typeName)
            => db.ProductTypes.Include(h => h.Products).Single(h => h.NameOfType.Equals(typeName)).Products;
    }
}
