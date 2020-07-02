using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Infrastructure
{
    public interface IRepository
    {
        
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void AddUser(User user);
        void AddOrder(Order order);
        void AddComment(Comment comment);
        void AddProductType(ProductType productType);

        List<ProductType> GetProductTypes();

        List<Order> GetOrdersFromUser(User userOwner);

        List<Comment> GetCommentsFromUser(User userOwner);
        List<Comment> GetCommentsFromProduct(int productId, int page, int count);

        Product[] GetProducts();
        Product[] GetProducts(int page, int count);
        List<Product> GetProductsFromUser(User userAuthor);
        List<Product> GetProductsByProductTypeName(string typeName, int page, int count);
        Product GetProductById(int id);
        List<Product> FindProductsByName(string partOfName, int page, int count);

        User[] GetUsers();
        User GetUserById(int id);

        Task SaveChangesAsync();
    }
}
