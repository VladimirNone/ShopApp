using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models.ViewModels;

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

        public void AddOrderedProduct(OrderedProduct orderedProduct)
            => shopDb.OrderedProducts.Add(orderedProduct);

        public void AddProduct(Product product)
            => shopDb.Products.Add(product);

        public void AddUser(User user)
            => shopDb.Users.Add(user);

        public void AddProductType(ProductType productType)
            => shopDb.ProductTypes.Add(productType);

        public List<Product> FindProductsByName(string partOfName, int page, int count)
            => shopDb.Products.Where(p => p.Name.Contains(partOfName)).Skip(page * count).Take(count).ToList();

        public List<Comment> GetCommentsFromProduct(int productId, int page, int count)
        {
            shopDb.Users.Load();
            return shopDb.Products.Include(h => h.Comments).Single(h => h.Id == productId).Comments.Where(h => h.CommentDeleted == false).SkipLast(page * count).TakeLast(count).Reverse().ToList();
        }

        public List<Comment> GetComments(int productId)
            => shopDb.Products.Include(h => h.Comments).Single(h => h.Id == productId).Comments.ToList();

        public List<Comment> GetCommentsFromUser(User userOwner)
            => shopDb.Users.Include(h=>h.Comments).Single(h => h.Id == userOwner.Id).Comments;

        public List<Order> GetOrdersFromUser(string userId)
            => shopDb.Users.Include(h => h.Orders).Single(h => h.Id == userId).Orders.Where(h => h.State == OrderState.Completed || h.State == OrderState.Confirmed).Distinct().ToList();

        public List<Order> GetProductsFromUserBasket(string userId)
            => shopDb.Users.Include(h => h.Orders).Single(h => h.Id == userId).Orders.Where(h => h.State == OrderState.IsBasket).Distinct().ToList();

        public List<OrderedProduct> GetProductsFromOrderByUser(string userId, int orderId)
            => shopDb.Users.Include(h => h.Orders).Single(h => h.Id == userId).Orders.Single(h => h.Id == orderId).OrderedProducts.Where(h => h.Cancelled == false).ToList();

        public Order GetUserOrder(string userId, int orderId)
        {
            shopDb.Products.Load();
            return shopDb.Orders.Include(h => h.OrderedProducts).SingleOrDefault(h => h.Id == orderId && h.CustomerId == userId && h.State == OrderState.Completed || h.State == OrderState.Confirmed);
        }

        public Order GetUserBasket(string userId)
        {
            shopDb.Products.Load();
            return shopDb.Orders.Include(h=>h.OrderedProducts).SingleOrDefault(h=>h.CustomerId == userId && h.State == OrderState.IsBasket);
        }

        public Order[] GetOrders()
            => shopDb.Orders.AsNoTracking().ToArray();

        public Product GetProductById(int id)
            => shopDb.Products.Include(h=>h.Publisher).Single(p => p.Id == id);

        public List<Product> GetProductsFromUser(User userAuthor)
            => shopDb.Users.Include(h => h.PublishedProducts).Single(h => h.Id == userAuthor.Id).PublishedProducts;

        public List<ProductType> GetProductTypes()
            => shopDb.ProductTypes.ToList();

        public Product[] GetProducts()
            => shopDb.Products.AsNoTracking().ToArray();

        public Product[] GetProducts(int page, int count)
            => shopDb.Products.AsNoTracking().Skip(page * count).Take(count).ToArray();

        public User[] GetUsers()
            => shopDb.Users.AsNoTracking().ToArray();

        public User GetUserById(int id)
            => shopDb.Users.Find(id);

        public User GetUserByUserName(string name)
            => shopDb.Users.AsNoTracking().SingleOrDefault(h => h.UserName == name);

        public List<Product> GetProductsByProductTypeName(string typeName, int page, int count)
            => shopDb.ProductTypes.Include(h => h.Products).Single(h => h.NameOfType.Equals(typeName)).Products.Skip(page * count).Take(count).ToList();

        public async Task SaveChangesAsync()
            => await shopDb.SaveChangesAsync();

        public void UpdateProduct(Product product)
            => shopDb.Update(product);

        public OrderedProduct GetOrderedProduct(int id)
            => shopDb.OrderedProducts.Single(h => h.Id == id);
    }
}
