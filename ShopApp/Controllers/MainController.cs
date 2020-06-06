using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.Controllers
{
    [Route("")]
    public class MainController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("product/{id:int}")]
        public IActionResult Product(int id)
        {
            return View();
        }
    }
}