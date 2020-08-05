using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.Controllers
{
    [Route("")]
    //[ResponseCache(CacheProfileName = "Caching")]
    public class MainController : Controller
    {
        [Route("")]
        [Route("category/{typeName}")]
        [Route("search/{typeName}")]
        [Route("category/{categoryName}/{productName}")]
        public IActionResult MainPage() 
        {
            return View();
        }

        [Route("product/{id:int}")]
        public IActionResult Product()
        {
            return View();
        }

        [Route("profile/{userId}")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("order/{orderId}")]
        public IActionResult Order()
        {
            return View();
        }
    }
}