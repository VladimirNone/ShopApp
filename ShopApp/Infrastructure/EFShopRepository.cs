using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ShopApp.Infrastructure
{
    public class EFShopRepository : IRepository
    {
        ShopAppDbContext shopDb { get; set; }
        public EFShopRepository(ShopAppDbContext shopDbContext)
        {
            shopDb = shopDbContext;
        }

        public void AddComment(Comment comment)
            => shopDb.Comments.Add(comment);

        public void AddOrder(Order order)
            => shopDb.Orders.Add(order);

        public void AddProduct(Product product)
            => shopDb.Products.Add(product);

        public void AddUser(User user)
            => shopDb.Users.Add(user);

        public void AddProductType(ProductType productType)
            => shopDb.ProductTypes.Add(productType);

        public List<Product> FindProductsByName(string partOfName)
            => shopDb.Products.Where(p => p.Name.Contains(partOfName)).ToList();

        public List<Comment> GetCommentsFromProduct(Product product)
            => shopDb.Products.Include(h => h.Comments).Single(h => h.Id == product.Id).Comments;

        public List<Comment> GetCommentsFromUser(User userOwner)
            => shopDb.Users.Include(h=>h.Comments).Single(h => h.Id == userOwner.Id).Comments;

        public List<Order> GetOrdersFromUser(User userOwner)
            => shopDb.Users.Include(h => h.Orders).Single(h => h.Id == userOwner.Id).Orders;

        public Product GetProductById(int id)
            => shopDb.Products.Single(p => p.Id == id);

        public List<Product> GetProductsFromUser(User userAuthor)
            => shopDb.Users.Include(h => h.PublishedProducts).Single(h => h.Id == userAuthor.Id).PublishedProducts;

        public List<ProductType> GetProductTypes()
            => shopDb.ProductTypes.ToList();

        public Product[] GetProducts()
            => shopDb.Products.AsNoTracking().ToArray();

        public User[] GetUsers()
            => shopDb.Users.AsNoTracking().ToArray();

        public User GetUserById(int id)
            => shopDb.Users.Find(id); 

        public List<Product> GetProductsByProductTypeName(string typeName)
            => shopDb.ProductTypes.Include(h => h.Products).Single(h => h.NameOfType.Equals(typeName)).Products;

        public async Task SaveChangesAsync()
            => await shopDb.SaveChangesAsync();

        public void UpdateProduct(Product product)
            => shopDb.Update(product);
    }
}
