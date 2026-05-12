using lab04.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace lab04.Controllers
{
    public class CategoryController : Controller
    {
        private AppDBContext _context;
        public CategoryController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchString, int pageNumber = 1)
        {
            int pageSize = 1;

            var category = _context.Categories.Include(x => x.Products).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                category = category.Where(p => p.Name.Contains(searchString));
                ViewBag.SearchString = searchString;
            }

            int totalItems = category.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedData = category
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
        public IActionResult Add(Category p)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(p);
                _context.SaveChanges();
                TempData["success"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.LOAI = _context.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        public IActionResult Edit(int id)
        {

            var product = _context.Categories.Find(id);
            if (product == null) return NotFound();

            ViewBag.LOAI = new SelectList(_context.Categories, "Id", "Name");
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(c);
                _context.SaveChanges();
                TempData["success"] = "Sửa sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.LOAI = _context.Categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return View();
        }
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                TempData["success"] = "Xoá sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
