using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace jewelry.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly jewelryContext _context;
        public CategoriesController(jewelryContext context, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        // GET: Categories
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            List<string> imagePathes = new List<string>();
            var categories = _context.Category.ToList();
            string path = null;
            List<Image> images = null;
            foreach (var cat in categories)
            {
                images = _context.Image.Where(a => a.Id.Equals(cat.ImageId)).ToList();
                if (images.Count != 0)
                {
                    path = images[0].imagePath;
                }

                imagePathes.Add(path);

                images = null;
            }
            ViewData["ImagePathes"] = imagePathes;
            return View(categories);
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ImageId")] Category category, List<IFormFile> postedFiles)
        {
            if (ModelState.IsValid)
            {
               
                string folder = "/lib/images/Categories/";
                string wwwRootpath = _hostEnvironment.WebRootPath + folder;
                string dir = wwwRootpath;
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                foreach (IFormFile postedFile in postedFiles)
                {
                    string categoryName = category.CategoryName;
                    categoryName = categoryName.Replace(" ", "");
                    string idString = Convert.ToString(category.Id);
                    categoryName = categoryName + idString;

                    using (FileStream stream = new FileStream(Path.Combine(wwwRootpath, categoryName + ".jpeg"), FileMode.Create))
                    {
                        postedFile.CopyTo(stream); //saving in the right folder

                        Image image = new Image();
                        image.imagePath = folder + categoryName + ".jpeg";
                        image.Name = categoryName;
                        _context.Add(image);
                         _context.SaveChanges();
                        category.ImageId = image.Id;
                    };
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id,string path)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["path"] = path;
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category, List<IFormFile> postedFiles)
        {

            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (postedFiles.Count() != 0)
                    {
                        string folder = "/lib/images/Categories/";
                        string wwwRootpath = _hostEnvironment.WebRootPath + folder;
                        string dir = wwwRootpath;
                        // If directory does not exist, create it
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        foreach (IFormFile postedFile in postedFiles)
                        {
                            string categoryName = category.CategoryName;
                            categoryName = categoryName.Replace(" ", "");
                            string idString = Convert.ToString(category.Id);
                            categoryName = categoryName + idString;

                            using (FileStream stream = new FileStream(Path.Combine(wwwRootpath, categoryName + ".jpeg"), FileMode.Create))
                            {
                                postedFile.CopyTo(stream); //saving in the right folder

                                Image image = _context.Image.Find(category.ImageId);
                                if(image!=null)
                                {
                                    image.imagePath = folder + categoryName + ".jpeg";
                                    _context.Update(image);
                                }
                                else
                                {
                                    image = new Image();
                                    image.imagePath = folder + categoryName + ".jpeg"; ;
                                    image.Name = categoryName;
                                    _context.Add(image);
                                    _context.SaveChanges();
                                }
                                category.ImageId = image.Id;
                            };
                        }
                    }
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
