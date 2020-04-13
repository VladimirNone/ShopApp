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
        void AddUser(User user);
        void AddOrder(Order order);
        void AddComment(Comment comment);

        List<ProductType> GetProductTypes();

        List<Order> GetOrdersFromUser(User userOwner);

        List<Comment> GetCommentsFromUser(User userOwner);
        List<Comment> GetCommentsFromProduct(Product product);

        List<Product> GetProductsFromUser(User userAuthor);
        List<Product> GetProductsByProductTypeName(string typeName);
        Product GetProductById(int id);
        List<Product> FindProductsByName(string partOfName);

        User GetUserById(int id);
        
    }
}
