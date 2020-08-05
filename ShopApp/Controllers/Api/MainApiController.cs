using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Infrastructure;
using ShopApp.Models;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.SignalR;
using ShopApp.Hubs;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using ShopApp.Modules.InnerModules;
using ShopApp.Modules.ExternalModules;

namespace ShopApp.Controllers.Api
{
    [Route("api")]
    public class MainApiController : Controller
    {
        public int CountOfProductsOnPage { get; } = 16;
        public IRepository repo { get; set; }
        public DataGenerator generator { get; set; }
        public IMemoryCache cache { get; set; }
        public IVerificationUserAccess verification {get;set;}
        public UserManager<User> userManager { get; set; }
        public IHubContext<NotifyHub> hub { get; set; }

        public MainApiController(IRepository repository, DataGenerator dataGenerator, IMemoryCache memoryCache, IHubContext<NotifyHub> hubContext, UserManager<User> userMng, IVerificationUserAccess vrf)
        {
            repo = repository;
            generator = dataGenerator;
            cache = memoryCache;
            hub = hubContext;
            userManager = userMng;
            verification = vrf;
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
            //Can use for waiting process icon
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

        [HttpGet("profile/{username?}")]
        public async Task<object> ViewUserData(string username)
        {
            User user = null;
            if (username == null)
                user = await userManager.GetUserAsync(HttpContext.User);
            else
                user = repo.GetUserByUserName(username);

            if (user == null)
                return StatusCode(423);
            return new { nickname = user.UserName, phoneNumber = user.PhoneNumber, user.DateOfRegistration, user.Email, user.EmailConfirmed };
        }

        [HttpGet("orders/{username?}")]
        public async Task<IActionResult> GetOrdersByUserId(string username)
        {
            (string id, bool res) = await verification.Verify(repo.GetUserByUserName(username), await userManager.GetUserAsync(HttpContext.User), userManager, new List<string>() { "admin" });

            if (res)
                return Ok(repo.GetOrdersFromUser(id));
            else
                return StatusCode(423);
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<Order> GetOrder(int orderId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return null;

            return repo.GetUserOrder(user.Id, orderId, false);
        }

        [HttpPost("add_comment")]
        public async Task<IActionResult> AddCommentToProduct(int productId, string commentBody)
        {
            var comment = new Comment() { AuthorId = (await userManager.GetUserAsync(HttpContext.User)).Id, Body = commentBody, ProductId = productId, TimePublished = DateTime.Now };
            repo.AddComment(comment);
            await repo.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> AddProductToBasket(int productId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return StatusCode(423);
            var basket = repo.GetUserBasket(user.Id);

            if (basket == null)
            {
                basket = new Order();
                basket.CustomerId = user.Id;

                repo.AddOrder(basket);
            }

            basket.OrderedProducts.Add(new OrderedProduct() { ProductId = productId, Count = 1 });
            await repo.SaveChangesAsync();
            return Ok();
        }
    }
}