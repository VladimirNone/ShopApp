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
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

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

        IWebHostEnvironment _appEnvironment;

        public MainApiController(IRepository repository, DataGenerator dataGenerator, IMemoryCache memoryCache, IHubContext<NotifyHub> hubContext, UserManager<User> userMng, IVerificationUserAccess vrf, IWebHostEnvironment appEnvironment)
        {
            repo = repository;
            generator = dataGenerator;
            cache = memoryCache;
            hub = hubContext;
            userManager = userMng;
            verification = vrf;
            _appEnvironment = appEnvironment;
        }

        public async void Foo()
        {
            await hub.Clients.All.SendAsync("receive", "data for you");
        }

        [HttpGet("categories")]
        public IActionResult GetProductTypes()
            => Ok(repo.GetProductTypes());

        [HttpGet("category/{typeName}/{page:int}")]
        public IActionResult GetProductsByType(string typeName, int page)
            => Ok(repo.GetProductsByProductTypeName(typeName, page, CountOfProductsOnPage));

        [HttpGet("search/{productName}/{page:int}")]
        public IActionResult GetProductsByName(string productName, int page)
            => Ok(repo.FindProductsByName(productName, page, CountOfProductsOnPage));

        [HttpGet("product/{id:int}")]
        public IActionResult GetProductById(int id)
            => Ok(repo.GetProductById(id));

        [HttpGet("prods/{page:int}")]
        public IActionResult GetProducts(int page)
        {
            //Can use for waiting process icon
            //Thread.Sleep(3000);

            var requestData = repo.GetProducts(page, CountOfProductsOnPage);

/*            if(page < 2)
            {
                for(int i = 0; i < 2; i++)
                {
                    if (!cache.TryGetValue("prods" + "\\" + page, out _))
                        cache.Set("prods" + "\\" + page, requestData);
                    else
                        return (IEnumerable<Product>)cache.Get("prods" + "\\" + page);
                    
                }
            }*/
            return Ok(requestData);
        }

        [HttpGet("comments/{prodId:int}/{page:int}")]
        public IActionResult GetCommentByProducts(int prodId, int page)
            => Ok(repo.GetCommentsFromProduct(prodId, page, 5));

        [Authorize]
        [HttpPost("add_comment")]
        public async Task<IActionResult> AddCommentToProduct(int productId, string commentBody)
        {
            var comment = new Comment() { AuthorId = (await userManager.GetUserAsync(HttpContext.User)).Id, Body = commentBody, ProductId = productId, TimePublished = DateTime.Now };
            repo.AddComment(comment);
            await repo.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpGet("profile/{username?}")]
        public async Task<object> ViewUserData(string username)
        {
            User user = null;
            if (username == null || username == "null")
                user = await userManager.GetUserAsync(HttpContext.User);
            else
                user = repo.GetUserByUserName(username);

            if (user == null)
                return StatusCode(423);
            return new { nickname = user.UserName, phoneNumber = user.PhoneNumber, user.DateOfRegistration, user.Email, user.EmailConfirmed };
        }

        [Authorize]
        [HttpGet("orders/{username?}")]
        public async Task<IActionResult> GetOrdersByUserId(string username)
        {
            (string id, bool res) = await verification.Verify(repo.GetUserByUserName(username), await userManager.GetUserAsync(HttpContext.User), userManager, new List<string>() { "admin" });

            if (res)
                return Ok(repo.GetOrdersFromUser(id));
            else
                return StatusCode(423);
        }

        [Authorize]
        [HttpGet("order/{orderId:int}")]
        public async Task<Order> GetOrder(int orderId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var order = repo.GetUserOrder(user.Id, orderId);
            order.OrderedProducts = order.OrderedProducts.Except(order.OrderedProducts.Where(h => h.Cancelled)).ToList();
            return order; 
        }

        [Authorize]
        [HttpPost("placeAnOrder")]
        public async Task<IActionResult> PlaceAnOrder()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var basket = repo.GetUserBasket(user.Id);
            basket.State = OrderState.Confirmed;
            basket.DateOfOrdering = DateTime.Now;
            await repo.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("orderedProduct/cancel")]
        public async Task<IActionResult> RemoveOrderedProductFromOrder(int orderedProductId, string userId)
        {
            (string id, bool res) = await verification.Verify(repo.GetUserById(userId), await userManager.GetUserAsync(HttpContext.User), userManager, new List<string>() { "admin" });

            if (res)
            {
                repo.GetOrderedProduct(orderedProductId).Cancelled = true;
                await repo.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("order/cancel")]
        public async Task<IActionResult> RemoveOrder(int orderId, string username)
        {
            (string id, bool res) = await verification.Verify(repo.GetUserByUserName(username), await userManager.GetUserAsync(HttpContext.User), userManager, new List<string>() { "admin" });

            if (res)
            {
                repo.GetUserOrder(id, orderId).State = OrderState.Cancelled;
                await repo.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("newProduct")]
        public async Task <IActionResult> AddNewProduct(IFormFile file, string product)
        {
            var prod = (Product)JsonSerializer.Deserialize(product, typeof(Product));
            string path = "/Images/";
            if (file != null)
            {
                path += Guid.NewGuid() +"."+ file.FileName.Split('.')[1];
                // сохраняем файл в папку Images в каталоге wwwroot
                using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            else
            {
                path += "no_foto.png";
            }

            if (prod.Count < 1)
                return BadRequest();

            prod.LinkToImage = path;
            prod.PublisherId = (await userManager.GetUserAsync(HttpContext.User)).Id;
            prod.TypeId = 1;
            repo.AddProduct(prod);
            await repo.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("basket")]
        public async Task<Order> GetBasket()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var basket = repo.GetUserBasket(user.Id);

            if (basket != null)
                basket.OrderedProducts = basket.OrderedProducts?.Except(basket.OrderedProducts.Where(h => h.Cancelled)).ToList();
            else
                basket = new Order() { OrderedProducts = new List<OrderedProduct>() };

            return basket;
        }

        [Authorize]
        [HttpPost("basket/cancel")]
        public async Task<IActionResult> RemoveOrderedProductFromBasket(int selectedProductId)
        {
            repo.GetOrderedProduct(selectedProductId).Cancelled = true;
            await repo.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("buy")]
        public async Task<IActionResult> AddProductToBasket(int productId, int count)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var basket = repo.GetUserBasket(user.Id);

            if (count < 1)
                return BadRequest();

            if (basket == null)
            {
                basket = new Order();
                basket.CustomerId = user.Id;
                basket.State = OrderState.IsBasket;

                repo.AddOrder(basket);
                await repo.SaveChangesAsync();
            }

            var prod = repo.GetProductById(productId);
            if (prod.Count < count)
                return BadRequest();
            else
                prod.Count -= count;

            if(basket.OrderedProducts?.Find(o=>o.ProductId == productId) == null)
                repo.AddOrderedProduct(new OrderedProduct() { ProductId = productId, Count = count, OrderId = basket.Id, TimeOfBuing = DateTime.Now });

            await repo.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("myProducts")]
        public async Task<IActionResult> GetUserProducts()
        {
            return Ok(repo.GetProductsFromUser(await userManager.GetUserAsync(HttpContext.User)));
        }

        [Authorize]
        [HttpPost("myProduct/cancel")]
        public async Task<IActionResult> RemoveUserProduct(int selectedProductId)
        {
            repo.GetProductById(selectedProductId).ProductDeleted = true;
            await repo.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("canSeeButtonRemove")]
        public async Task<IActionResult> CanSeeButtonRemove(string username)
        {
            (string id, bool res) = await verification.Verify(repo.GetUserByUserName(username), await userManager.GetUserAsync(HttpContext.User), userManager, new List<string>() { "admin" });

            return Ok(res);
        }

    }
}