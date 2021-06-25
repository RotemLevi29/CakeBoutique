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
            return View(_context.Category.Include(a=>a.image).ToList());
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
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category, IFormFile postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image image = new Image();
                        image.Name = "Banner";
                        postedFile.CopyTo(ms);
                        category.image = image;
                        category.image.image = ms.ToArray();
                        _context.Image.Add(category.image);
                        _context.Add(category);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _context.Category.Include(a=>a.image).Where(a=>a.Id.Equals(id)).First();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category, IFormFile postedFile)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    //deleting the old banner
                    category = _context.Category.Include(a => a.image).Where(a => a.Id.Equals(id)).First();
                    if (category.image != null)
                    {
                        (new ImagesController(_context)).regularDelete(category.image.Id);
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image image = new Image();
                        image.Name = "Banner";
                        postedFile.CopyTo(ms);
                        category.image = image;
                        category.image.image = ms.ToArray();
                        _context.Image.Add(category.image);
                        _context.Update(category);
                        await _context.SaveChangesAsync();
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
            var category =  _context.Category.Include(a => a.image).Where(a => a.Id.Equals(id)).First();
            if (category.image != null)
            {
                (new ImagesController(_context)).regularDelete(id);
            }
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
