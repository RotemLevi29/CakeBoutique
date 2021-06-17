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
            Dictionary<string,int> pathes = new Dictionary<string, int>();
            List<CarouselImage> carouselImages = (List<CarouselImage>)_context.CarouselImage.ToList();
            foreach(CarouselImage carImg in carouselImages)
            {
                try
                {
                    pathes.Add(_context.Image.Where(a => a.Id.Equals(carImg.CarImageId)).FirstOrDefault().imagePath,carImg.Id);
                }
                catch{}

                }
            ViewBag.pathes = pathes;
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
        public async Task<IActionResult> Create([Bind("Id,Width,Height")] CarouselImage carouselImage, List<IFormFile> postedFiles)
        {
            if(carouselImage.Width == 0 || carouselImage.Height == 0)//using default sizes
                    {
                carouselImage.Width = _context.CarouselImage.FirstOrDefault().Width;
                carouselImage.Height = _context.CarouselImage.FirstOrDefault().Height;
                    }
            

            if (ModelState.IsValid)
            {
                string folder = "/lib/images/MainCarousel/";
                string wwwRootpath = _hostEnvironment.WebRootPath + folder;
                string dir = wwwRootpath;
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                foreach (IFormFile postedFile in postedFiles)
                {
                    string originalName = postedFile.FileName.Replace(" ", "");
                    //string filename = Path.GetFileName(postedFile.FileName);
                    int imageNumber = _context.CarouselImage.Count();
                    string imageName = "carousel" + imageNumber.ToString() + originalName + ".jpeg";
                    using (FileStream stream = new FileStream(Path.Combine(wwwRootpath, imageName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream); //saving in the right folder

                        Image image = new Image();
                        image.imagePath = folder + imageName;
                        image.Name = imageName;
                        image.ProductId = null;//carousel is 0
                        image.Type = Image.ImageType.Carousel;
                        _context.Add(image);
                        await _context.SaveChangesAsync();
                        carouselImage.CarImageId = image.Id;
                    };
                    _context.Add(carouselImage);
                    await _context.SaveChangesAsync();
                    //    return RedirectToAction(nameof(Index));
                    return RedirectToAction(nameof(Index));

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
            var carouselImage = await _context.CarouselImage.FindAsync(id);
            //remove the image itself
            var images = _context.Image.Where(a => a.Id.Equals(carouselImage.CarImageId)).ToList();
            foreach(var img in images)
            {
                new ImagesController(_context).regularDelete(img.Id, _hostEnvironment.WebRootPath);
            }
            _context.CarouselImage.Remove(carouselImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CarouselImageExists(int id)
        {
            return _context.CarouselImage.Any(e => e.Id == id);
        }
        public List<string> getImages()
        {
            List<string> pathes = new List<string>();
           var imageid = _context.CarouselImage.Select(column => column.CarImageId);
            foreach(var id in imageid)
            {
                    pathes.Add(_context.Image.Where(a => a.Id == id).FirstOrDefault().imagePath);
            }
            return pathes;
        }
    }
}
