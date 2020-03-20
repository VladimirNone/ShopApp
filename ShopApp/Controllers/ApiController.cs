using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Infrastructure;
using ShopApp.Models;
using System.Text.Json;

namespace ShopApp.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        public IRepository repo { get; set; }

        public ApiController(IRepository repository)
        {
            repo = repository;
        }

        [HttpGet("prods")]
        public List<Product> GetProducts()
        {
            var r = repo.GetProductsByProductType(new ProductType() { NameOfType = "Cars", Id = 7 });
            r.AddRange(repo.GetProductsByProductType(new ProductType() { NameOfType = "Cars", Id = 7 }));
            return r;
        }
    }
}