using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Infrastructure;
using ShopApp.Models;
using System.Text.Json;
using ShopApp.ExternalModules;

namespace ShopApp.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        public IRepository repo { get; set; }
        public DataGenerator generator { get; set; }

        public ApiController(IRepository repository, DataGenerator dataGenerator)
        {
            repo = repository;
            generator = dataGenerator;
        }

        [HttpGet("categories")]
        public IEnumerable<ProductType> GetProductTypes()
            => repo.GetProductTypes();

        [HttpGet("category/{typeName}/{page:int}")]
        public IEnumerable<Product> GetProductsByType(string typeName, int page)
            => repo.GetProductsByProductTypeName(typeName);

        [HttpGet("search/{productName}/{page:int}")]
        public IEnumerable<Product> GetProductsByName(string productName, int page)
        {
            if (page == 1)
                return new List<Product>();//testing
            return repo.FindProductsByName(productName);
        }

        [HttpGet("product/{id:int}")]
        public Product GetProductById(int id)
            => repo.GetProductById(id); 

        [HttpGet("prods/{page:int}")]
        public IEnumerable<Product> GetProducts(int page)
        {            
            var r = repo.GetProducts();
            return r;
        }

    }
}