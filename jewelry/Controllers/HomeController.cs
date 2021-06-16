using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger, jewelryContext context)
        {
            _context = context;
            _logger = logger;
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
