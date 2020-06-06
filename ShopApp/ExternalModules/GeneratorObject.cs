﻿using Bogus;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.ExternalModules
{
    public class ObjectGenerator
    {
        public static Faker<Comment> GenerateComment(Product[] products, User[] authors)
            => new Faker<Comment>("ru")
                .RuleFor(h => h.AuthorId, g => g.Random.ArrayElement(authors).Id)
                .RuleFor(h => h.ProductId, g => g.Random.ArrayElement(products).Id)
                .RuleFor(h => h.TimePublished, (g, o) => g.Date.Between((products.First(j => j.Id == o.ProductId).DateOfPublication < authors.First(j => j.Id == o.AuthorId).DateOfRegistration) ? products.First(j => j.Id == o.ProductId).DateOfPublication : authors.First(j => j.Id == o.AuthorId).DateOfRegistration, DateTime.Now))
                .RuleFor(h => h.Body, g => g.Lorem.Paragraph());

        public static Faker<Product> GenerateProduct(User[] publishers, ProductType[] types)
            => new Faker<Product>("ru")
                .RuleFor(h => h.Count, g => g.Random.Number(0, 1000))
                .RuleFor(h => h.Description, g => g.Lorem.Paragraph())
                .RuleFor(h => h.LinkToImage, g => Path.Combine("Images", g.Random.Number(0, Directory.GetFiles("Images", "*.jpg").Length - 1) + ""))
                .RuleFor(h => h.Name, g => g.Commerce.ProductName())
                .RuleFor(h => h.Price, g => Math.Round(g.Random.Double() + g.Random.Number(0, 1000), 2))
                .RuleFor(h => h.PublisherId, g => g.Random.ArrayElement(publishers).Id)
                .RuleFor(h => h.TypeId, g => g.Random.ArrayElement(types).Id)
                .RuleFor(h => h.DateOfPublication, (g, o) => g.Date.Between(publishers.First(j => j.Id == o.PublisherId).DateOfRegistration, DateTime.Now));

        public static Faker<Order> GenerateOrder(User[] customers, Product[] products)
            => new Faker<Order>("ru")
                .RuleFor(h => h.Product, g => g.Random.ArrayElement(products))
                .RuleFor(h => h.Customer, g => g.Random.ArrayElement(customers))
                .RuleFor(h => h.TimeOfBuing, (g, o) => g.Date.Between(products.First(j=>j.Id == o.ProductId).DateOfPublication, DateTime.Now));

        public static Faker<User> GenerateUser(UserManager<User> manager)
            => new Faker<User>("ru")
                .RuleFor(h => h.DateOfRegistration, g => g.Date.Between(new DateTime(2015, 10, 10), DateTime.Now))
                .RuleFor(h => h.Email, g => g.Person.Email)
                .RuleFor(h => h.UserName, g => g.Person.UserName)
                .FinishWith(async (g, o) => await manager.CreateAsync(o, g.Internet.Password()));

        public static Faker<ProductType> GenerateProductType()
            => new Faker<ProductType>("ru")
                .RuleFor(h => h.NameOfType, g => g.Commerce.Categories(1)[0]);
    }
}