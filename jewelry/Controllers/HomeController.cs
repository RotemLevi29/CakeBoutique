using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Controllers
{
    public class HomeController : Controller
    {
        private readonly jewelryContext _context;
        private readonly ILogger<HomeController> _logger;
        Dictionary<int, string> _categories;
        public HomeController(ILogger<HomeController> logger, jewelryContext context)
        {
            _context = context;
            _logger = logger;
            _categories = new Dictionary<int, string>();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult facebook()
        {
            return View();
        }

        public IActionResult SearchPage()
        {
            SelectList selectListCategories = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            ViewData["CategoryList"] = selectListCategories;
            return View();
        }



        public IActionResult Statistics()
        {
            List<Category> categories = _context.Category.ToList();

            var result = (from o in _context.ProductCart
                          group o by o.ProductId into o
                          orderby o.Sum(c => c.Quantity) descending
                          select new { o.Key, Total = o.Sum(c => c.Quantity) }).FirstOrDefault();
            var bestProduct = _context.Product.Find(result.Key);
            if (bestProduct != null)
            {
                ViewData["bestproductname"] = bestProduct.ProductName;
            }

            return View(categories);
        }

        private void getCategories()
        {
            if (_categories.Count() == 0)
            {
                SelectList selectListCategories = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
                var categoriesId = _context.Category.Select(column => column.Id).ToList();
                foreach (var catId in categoriesId)
                {
                    string cat = selectListCategories.Where(a => a.Value.Equals(catId.ToString())).FirstOrDefault().Text;
                    if (cat != null)
                    {
                        _categories.Add(catId, cat);
                    }
                }

            }
        }
        public IActionResult Index()
        {
            List<string> imagePathes = new List<string>();
            var categories = _context.Category.ToList();
            string path = null;
            List<Image> images = null;
            foreach(var cat in categories)
            {
                images =_context.Image.Where(a => a.Id.Equals(cat.ImageId)).ToList();
                if (images.Count!=0)
                {
                    path = images[0].imagePath;
                }
             
                imagePathes.Add(path);

                images = null;
            }
            ViewData["ImagePathes"] = imagePathes;
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
