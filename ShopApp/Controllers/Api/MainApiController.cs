using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Infrastructure;
using ShopApp.Models;
using System.Text.Json;
using ShopApp.ExternalModules;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.SignalR;
using ShopApp.Hubs;
using Microsoft.AspNetCore.Identity;

namespace ShopApp.Controllers.Api
{
    [Route("api")]
    public class MainApiController : Controller
    {
        public int CountOfProductsOnPage { get; } = 16;
        public IRepository repo { get; set; }
        public DataGenerator generator { get; set; }
        public IMemoryCache cache { get; set; }
        public UserManager<User> userManager { get; set; }
        public IHubContext<NotifyHub> hub { get; set; }

        public MainApiController(IRepository repository, DataGenerator dataGenerator, IMemoryCache memoryCache, IHubContext<NotifyHub> hubContext, UserManager<User> userMng)
        {
            repo = repository;
            generator = dataGenerator;
            cache = memoryCache;
            hub = hubContext;
            userManager = userMng;
        }

        public async void Foo()
        {
            await hub.Clients.All.SendAsync("receive", "data for you");
        }

        [HttpGet("categories")]
        public IEnumerable<ProductType> GetProductTypes()
            => repo.GetProductTypes();

        [HttpGet("category/{typeName}/{page:int}")]
        public IEnumerable<Product> GetProductsByType(string typeName, int page)
            => repo.GetProductsByProductTypeName(typeName, page, CountOfProductsOnPage);

        [HttpGet("search/{productName}/{page:int}")]
        public IEnumerable<Product> GetProductsByName(string productName, int page)
            => repo.FindProductsByName(productName, page, CountOfProductsOnPage);

        [HttpGet("product/{id:int}")]
        public Product GetProductById(int id)
            => repo.GetProductById(id);

        [HttpGet("prods/{page:int}")]
        public IEnumerable<Product> GetProducts(int page)
        {
            //Can use for waiting process bar
            //Thread.Sleep(3000);

            var requestData = repo.GetProducts(page, CountOfProductsOnPage);

            if(page < 2)
            {
                for(int i = 0; i < 2; i++)
                {
                    if (!cache.TryGetValue("prods" + "\\" + page, out _))
                        cache.Set("prods" + "\\" + page, requestData);
                    else
                        return (IEnumerable<Product>)cache.Get("prods" + "\\" + page);
                    
                }
            }
            return requestData;
        }
        
        [HttpGet("comments/{prodId:int}/{page:int}")]
        public IEnumerable<Comment> GetCommentByProducts(int prodId, int page)
            => repo.GetCommentsFromProduct(prodId, page, 5); 

        [HttpGet("profile/{userId}")]
        public async Task<object> ViewUserData(string userId)
        {
            //var user = await userManager.FindByIdAsync(userId);
            var user = await userManager.GetUserAsync(HttpContext.User);
            return new { nickname = user.UserName, phoneNumber = user.PhoneNumber, user.DateOfRegistration, user.Email, user.EmailConfirmed };
        }
    }
}