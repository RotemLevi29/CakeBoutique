using CakeBoutique.Data;
using CakeBoutique.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CakeBoutique.Controllers
{
    public class HomeController : Controller
    {
        private readonly CakeBoutiqueContext _context;
        private readonly ILogger<HomeController> _logger;
        Dictionary<int, string> _categories;
        public HomeController(ILogger<HomeController> logger, CakeBoutiqueContext context)
        {
            _context = context;
            _logger = logger;
            _categories = new Dictionary<int, string>();
        }

        //Get:Facebook post page
        [Authorize(Roles = "Admin")]
        public IActionResult facebook()
        {
            return View();
        }

        //We don't using this page right now
        public IActionResult SearchPage()
        {
            SelectList selectListCategories = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            ViewData["CategoryList"] = selectListCategories;
            return View();
        }


        //Get:Statistics page
        /**
         * This function displaying some statistics data about the webstie,
         * first it making sql query from ProductCart table, actualyy grouping
         * all the productcarts from the same type and sum the quantity, than ordering 
         * by descending so the most attractive product will be in the top.
         *It also sending the categories list to the page, in every category there an insterest
         *variable which updating when someone pressing the category so the manager of the site
         *can understang in which categories to invest more.
         */
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

        //Get list of categories, using in other functions.
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
           
            return View(_context.Category.Include(a=>a.image).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
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
