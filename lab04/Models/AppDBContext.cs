using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab04.Models
{
        public class AppDBContext : DbContext
        {
            public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    Seed Data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện thoại" },
                new Category { Id = 2, Name = "Laptop" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone", Price = 1000, CategoryId = 1 },
                new Product { Id = 2, Name = "Dell", Price = 1500, CategoryId = 2 },
                new Product { Id = 3, Name = "Lenovo", Price = 1200, CategoryId = 2 },
                new Product { Id = 4, Name = "Mac Book", Price = 2000, CategoryId = 2 }
            );
        }
    }
}
