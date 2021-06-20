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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Web;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace jewelry.Controllers
{
    public class ProductsController : Controller
    {
        private readonly jewelryContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        Dictionary<int, string> _categories;
        public ProductsController(jewelryContext context,IWebHostEnvironment hostEnvironment)
        {
           _categories = new Dictionary<int, string>();

            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        
        // GET: Catergory Products
        public async Task<IActionResult> CategoryPage(int categoryId)
        {
            Category category = _context.Category.Find(categoryId);
            if (category != null)
            {
                category.Interest++;
                await _context.SaveChangesAsync();
            }
            List<Product> products = _context.Product.Where(a => a.CategoryId.Equals(categoryId)).ToList();
            List<string> pathes = new List<string>();
            foreach (Product p in products)
            {
                string path = (_context.Image.Where(a => a.ProductId.Equals(p.Id)).
                    Select(column => column.imagePath).FirstOrDefault());
                pathes.Add(path);
            }
            ViewData["pathes"] = pathes;
            Tuple<List<Product>, List<string>> tuple = new Tuple<List<Product>, List<string>>(products, pathes); 
            return View(nameof(CategoryPage), tuple);
        }


        // GET: Products
        [HttpGet]
        public async Task<IActionResult> ProductPage(int? id)
        {
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                List<string> pathes =_context.Image.Where(a => a.ProductId.Equals(id)).Select(column=>column.imagePath).ToList();
                Tuple<Product, List<string>> tuple = new Tuple<Product, List<string>>(product, pathes);
                return View(tuple);
            }
        }



        // GET: Products
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            SelectList selectListCategories = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            getCategories();
            ViewData["CategoryList"] = _categories;
            ViewData["Categries"] = selectListCategories;
            return View(await _context.Product.ToListAsync());
        }



        // GET: Products/Details/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create

        [Authorize(Roles="Admin,Editor")]
        public IActionResult Create()
        {
            ViewData["Categries"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Create([Bind("Id,ProductName,Price,Description,Type,CategoryId,Discount,RateSum,Rates,Orders,StoreQuantity,NameOption")] Product product, List<IFormFile> postedFiles)
        {
            if (postedFiles.Count() > 3)
            {
                ViewData["error"] = "You can upload only 3 images!";
                return View();
            }
         // WE NEED TO TAKE CARE TO THE IMAGE TYPE 

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                //images
                string folder = "/lib/images/products/";
                string wwwRootpath = _hostEnvironment.WebRootPath + folder;
                string dir = wwwRootpath;
                // If directory does not exist, create it
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                int i = 0;
                foreach (IFormFile postedFile in postedFiles)
                {
                    string productName = product.ProductName;
                    productName = productName.Replace(" ", "");
                    string idString = Convert.ToString(product.Id);
                    productName = productName + idString + i.ToString();
                    using (FileStream stream = new FileStream(Path.Combine(wwwRootpath, productName + ".jpeg"), FileMode.Create))
                    {
                        postedFile.CopyTo(stream); //saving in the right folder

                        Image image = new Image();
                        image.imagePath = folder + productName + ".jpeg";
                        image.Name = productName;
                        image.ProductId = product.Id;
                        _context.Add(image);
                    };
                    await _context.SaveChangesAsync();
                    i++;
                }


                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Products/Edit/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categries"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Price,Description,Type,CategoryId,Discount,RateSum,Rates,Orders,StoreQuantity,NameOption")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        private void getCategories()
        {
            if(_categories.Count()==0)
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
        /**
         * type = 0 : input is product name
         * type = 1 : input is product price
         * type = 2 : input is product type
         */
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Search(string input,string type)
        {
            if (_categories.Count() == 0)
            {
                getCategories();
            }
            ViewData["CategoryList"] = _categories;
            switch (type)
            {
                case "0":
                    {
                        if(input != null)
                        { 
                        return PartialView(await _context.Product.Where(a => a.ProductName.Contains(input)).ToListAsync());
                        }
                        else
                        {
                            return PartialView(await _context.Product.ToListAsync());
                        }
                    }
                case "1":
                    {
                        double price = Double.Parse(input);
                        return PartialView(await _context.Product.Where(a => a.Price.Equals(price)).ToListAsync());
                    }
                case "2":
                    {
                        int theType = Int32.Parse(input);
                         return PartialView(await _context.Product.Where(a => a.CategoryId.Equals(theType)).ToListAsync());
                    }
            }
            return null;
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            var productImages = _context.Image.Where(a => a.ProductId.Equals(id)).ToList();
            foreach(var image in productImages)
            {
                 new ImagesController(_context).regularDelete(image.Id, _hostEnvironment.WebRootPath);
            }
             _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Editor")]
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
