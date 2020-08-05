using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Identity;
using ShopApp.Infrastructure;
using ShopApp.Models;

namespace ShopApp.Modules.ExternalModules
{
    public class DataGenerator
    {
        IRepository repo { get; set; }
        UserManager<User> userManager { get; set; }
        public DataGenerator(IRepository repository, UserManager<User> manager)
        {
            repo = repository;
            userManager = manager; 
        }

        public IEnumerable<Comment> GenerateComments(int count)
            => ObjectGenerator.GenerateComment(repo.GetProducts(0, -1), userManager.Users.ToArray()).Generate(count);

        public IEnumerable<Product> GenerateProducts(int count)
            => ObjectGenerator.GenerateProduct(userManager.Users.ToArray(), repo.GetProductTypes().ToArray()).Generate(count);

        public IEnumerable<User> GenerateUsers(int count)
            => ObjectGenerator.GenerateUser(userManager).Generate(count);

        public IEnumerable<OrderedProduct> GenerateOrderedProducts(int count)
            => ObjectGenerator.GenerateOrderedProduct(repo.GetOrders(), repo.GetProducts()).Generate(count);

        public IEnumerable<Order> GenerateOrders(int count)
            => ObjectGenerator.GenerateOrder( userManager.Users.ToArray()).Generate(count);

        public IEnumerable<ProductType> GenerateProductTypes(int count)
            => ObjectGenerator.GenerateProductType().Generate(count);
    }
}
