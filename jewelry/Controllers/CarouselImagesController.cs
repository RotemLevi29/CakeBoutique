using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace jewelry.Controllers
{

    public class CarouselImagesController : Controller
    {
        private readonly jewelryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CarouselImagesController(jewelryContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }


        // GET: CarouselImages
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
           
           return View(await _context.CarouselImage.ToListAsync());
        }

        // GET: CarouselImages/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carouselImage = await _context.CarouselImage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carouselImage == null)
            {
                return NotFound();
            }

            return View(carouselImage);
        }

        // GET: CarouselImages/Create
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            ViewData["count"] = _context.CarouselImage.Count();
            return View();
        }

        // POST: CarouselImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id")] CarouselImage carouselImage, IFormFile postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        postedFile.CopyTo(ms);
                        carouselImage.Image = ms.ToArray();
                        _context.Add(carouselImage);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: CarouselImages/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carouselImage = await _context.CarouselImage.FindAsync(id);
            if (carouselImage == null)
            {
                return NotFound();
            }
            return View(carouselImage);
        }

        // POST: CarouselImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Width,Height")] CarouselImage carouselImage)
        {
            if (id != carouselImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carouselImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarouselImageExists(carouselImage.Id))
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
            return View(carouselImage);
        }

        //Preview
        [HttpGet]
        public IActionResult Preview()
        {
            return View();
        }

        // GET: CarouselImages/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carouselImage = await _context.CarouselImage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carouselImage == null)
            {
                return NotFound();
            }

            return View(carouselImage);
        }

        // POST: CarouselImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CarouselImage carousel = _context.CarouselImage.Where(a=>a.Id.Equals(id)).FirstOrDefault();
            if (carousel != null)
            {
                _context.CarouselImage.Remove(carousel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool CarouselImageExists(int id)
        {
            return _context.CarouselImage.Any(e => e.Id == id);
        }
     
    }
}
