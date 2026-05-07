using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab04.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace lab04.Controllers
{
    public class ProductController : Controller
    {
        private AppDBContext _context;
        public ProductController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 1; 

            var products = _context.Products.Include(x => x.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
                ViewBag.SearchString = searchString; 
            }

            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedData = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(pagedData);
        }
        public IActionResult Add()
        {
            ViewBag.LOAI = _context.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product p)
        {
            if(ModelState.IsValid)
            {
                _context.Products.Add(p);
                _context.SaveChanges();
                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.LOAI = _context.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        public IActionResult Edit(int id)
        {

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.LOAI = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(p);
                _context.SaveChanges();
                TempData["success"] = "Sửa sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.LOAI = _context.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["success"] = "Xoá sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult Statistics()
        {
            var stats = _context.Products
                .Include(p => p.Category)
                .GroupBy(p => p.Category.Name)
                .Select(g => new CategoryStatsViewModel
                {
                    CategoryName = g.Key,
                    ProductCount = g.Count(),
                    MaxPrice = (decimal)g.Max(p => p.Price),
                    MinPrice = (decimal)g.Min(p => p.Price),
                    AveragePrice = (decimal)g.Average(p => p.Price),
                    TotalValue = (decimal)g.Sum(p => p.Price)
                })
                .ToList();

            return View(stats);
        }
    }
}
