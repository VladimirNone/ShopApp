using Microsoft.EntityFrameworkCore;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Infrastructure
{
    public class ShopAppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AccountData> AccountDatas { get; set; }

        public ShopAppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.Comments).HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(o => o.Product).WithMany(p => p.Orders).HasForeignKey(o => o.ProductId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>().HasOne(o => o.Customer).WithMany(u => u.Orders).HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasOne(u => u.AccountData).WithOne(a => a.User).HasForeignKey<User>(u => u.AccountDataId);
        }

        public void SeedData()
        {
            var productTypes = new[]
            {
                new ProductType(){NameOfType = "Cars"},
                new ProductType(){NameOfType = "Phones"},
                new ProductType(){NameOfType = "Laptops"},
            };
            ProductTypes.AddRange(productTypes);

            var users = new[]{
                new User(){Nick = "User1", AccountData = new AccountData(){  } },
                new User(){Nick = "User2", AccountData = new AccountData(){  }},
                new User(){Nick = "User3", AccountData = new AccountData(){  }},
            };
            Users.AddRange(users);

            var products = new[]
            {
                new Product(){Name = "BMW X6", Publisher = users[0], Type = productTypes[0], LinkToImage="~/wwwroot/img/18447504-1494996660534974-7136101589135740802-n.png"},
                new Product(){Name = "Fiat Siena", Publisher = users[0], Type = productTypes[0], LinkToImage="~/wwwroot/img/20181017224605_11247.jpg"},
                new Product(){Name = "Fiat Doble", Publisher = users[0], Type = productTypes[0], LinkToImage="~/wwwroot/img/71avxGd2SZL._SY355_.jpg"},
                new Product(){Name = "iPhone 10X", Publisher = users[0], Type = productTypes[1], LinkToImage="~/wwwroot/img/71M7vg8yj9L._SY355_.jpg"},
                new Product(){Name = "iPhone 8", Publisher = users[1], Type = productTypes[1], LinkToImage="~/wwwroot/img/product.png"},
                new Product(){Name = "Samsung X1560", Publisher = users[1], Type = productTypes[2], LinkToImage="~/wwwroot/img/product.png"},
                new Product(){Name = "Lenovo agV40", Publisher = users[2], Type = productTypes[2], LinkToImage="~/wwwroot/img/Whitening-face-care-products-set-in-stock.png_350x350.png"},
            };
            Products.AddRange(products);

            var comments = new[]
            {
                new Comment(){Body = "Good thing", Author = users[0], Product = products[0]},
                new Comment(){Body = "Is's great", Author = users[2], Product = products[1]},
                new Comment(){Body = "It's amazing", Author = users[1], Product = products[5]},
                new Comment(){Body = "I don't like such products", Author = users[1], Product = products[6]},
                new Comment(){Body = "Simple in use", Author = users[1], Product = products[2]},
                new Comment(){Body = "Broke down yestedey, dislike", Author = users[0], Product = products[2]},
            };
            Comments.AddRange(comments);

            var orders = new[]
            {
                new Order(){Customer = users[0], Product = products[6]},
                new Order(){Customer = users[2], Product = products[5]},
                new Order(){Customer = users[1], Product = products[1]},
                new Order(){Customer = users[0], Product = products[2]},
                new Order(){Customer = users[0], Product = products[0]},
            };
            Orders.AddRange(orders);

            SaveChanges();
        }
    }
}
