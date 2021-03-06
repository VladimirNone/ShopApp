﻿using System;
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

        [Route("profile/{userName}")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("order/{orderId}")]
        public IActionResult Order()
        {
            return View();
        }

        [Route("basket")]
        public IActionResult Basket()
        {
            return View();
        }

        [Route("new_product")]
        public IActionResult NewProduct()
        {
            return View();
        }

        [Route("user_products")]
        public IActionResult UserProducts()
        {
            return View();
        }

        [Route("weatherforecast")]
        public IActionResult Foo()
        {
            return RedirectToActionPermanent("MainPage");
        }
    }
}