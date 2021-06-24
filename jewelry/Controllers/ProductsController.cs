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
        //Display page with products with categoryId type(necklace,ring.......)
        public async Task<IActionResult> CategoryPage(int categoryId)
        {
            Category category = _context.Category.Include(a => a.products).Where(a => a.Id.Equals(categoryId)).First();
            if (category != null)
            {
                category.Interest++;
                await _context.SaveChangesAsync();
            }
            List<Product> products = category.products;
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


        // GET: ProductPage
        //this is a productpage with option to add to cart
        //it sending the product and list of pathes of it's images
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
        //Display all the product to the admin,editor with search options
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            SelectList selectListCategories = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            getCategories();
            ViewData["CategoryList"] = _categories;
            ViewData["Categories"] = selectListCategories;

            //images
            List<Product> products = _context.Product.Include(a=>a.Images).Take(10).ToList();
            List<string> pathes = new List<string>();
            foreach(var pro in products)
            {
/*                Image image = _context.Image.Where(a => a.ProductId.Equals(pro.Id)).FirstOrDefault();
*/                if (pro.Images != null)//if there are images take the first
                {
                    pathes.Add(pro.Images[0].imagePath);
                }
            }
            ViewData["pathes"] = pathes;
            return View(products);
        }



        // GET: Products/Details/5
        //Display the product details for the admin.editor
        

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
        //creating a product with maximum 3 picturs
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
            if (postedFiles.Count() == 0)
            {
                ViewData["error"] = "You must upload atleast 1 image";
                return View();
            }
            //if the data is valid creating product and adding it to database
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
        //Displaying the Edit page with product details and image
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int? id,string path)
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
            ViewData["path"] = path;
            ViewData["Categries"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.CategoryName));
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //updating the product details and updating also the images
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Price,Description,Type,CategoryId,Discount,RateSum,Rates,Orders,StoreQuantity,NameOption")] Product product, List<IFormFile> postedFiles)
        {
            ViewData["error"] = "";
            if (id != product.Id)
            {
                return NotFound();
            }
            if (postedFiles.Count() > 3)
            {
                ViewData["error"] = "You can uploade maximum 3 images";
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                        if (!postedFile.ContentType.Contains("image")) //make sure this is an image
                        {
                            continue;
                        }
                        string productName = product.ProductName;
                        productName = productName.Replace(" ", "");
                        string idString = Convert.ToString(product.Id);
                        Random rnd = new Random();
                        string random = (rnd.Next(1, 9999)).ToString();
                        productName = productName + idString + i.ToString() +random;
                        using (FileStream stream = new FileStream(Path.Combine(wwwRootpath, productName + ".jpeg"), FileMode.Create))
                        {
                            postedFile.CopyTo(stream); //saving in the right folder

                            List<Image> images = _context.Image.Where(a => a.ProductId.Equals(product.Id)).ToList();
                            foreach (var image in images)
                            {
                                if (System.IO.File.Exists(image.imagePath))
                                {
                                    System.IO.File.Delete(image.imagePath);
                                }
                                _context.Remove(image);
                            }
                            Image newImage = new Image();

                            newImage.imagePath = folder + productName + ".jpeg";
                            newImage.Name = productName;
                            newImage.ProductId = product.Id;
                            _context.Add(newImage);
                        };
                    }
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
       
        public IActionResult SearchProductClientFrame()
        {
            return PartialView();
        }

    public async Task<IActionResult> SearchProductsClient(string input, string category,string maxprice)
            {
            int cat = Int32.Parse(category);
            double price = Double.Parse(maxprice);
            List<Product> products;
            List<string> pathes = new List<string>();
            ViewData["searchedInput"]='"' + input + '"';
            ViewData["result"] = "";
            ViewData["error"] = "";
            if (input == null)
            {
                input = "";
            }
            if(price!=0 && cat != 0)
            {
                products = _context.Product.Where(a =>
        (a.ProductName.Contains(input) || a.Description.Contains(input)) &&
        (a.Price < price) && (a.CategoryId.Equals(cat))).ToList();
            }
            if (price == 0 && cat!=0)
            {
                products = _context.Product.Where(a =>
                (a.ProductName.Contains(input) || a.Description.Contains(input)) &&
                (a.CategoryId.Equals(cat))).ToList();
            }
            else if (cat == 0 && price!=0)
            {
                products = _context.Product.Where(a =>
                                (a.ProductName.Contains(input) || a.Description.Contains(input)) &&
                                (a.Price < price)).ToList();
            }
            else
            {
                products = _context.Product.Where(a =>
        (a.ProductName.Contains(input) || a.Description.Contains(input))).ToList();
            }
          
            if (products.Count() == 0)
            {
                ViewData["error"] = "error";
                products = _context.Product.Take(10).ToList();
            }
            foreach (var pro in products)
            {
                var image = _context.Image.Where(a => a.ProductId.Equals(pro.Id)).First();
                pathes.Add(image.imagePath);
            }
            ViewData["pathes"] = pathes;

            return PartialView("SearchProductsClient", products);
        }



        /**
         * type = 0 : input is product name
         * type = 1 : input is product price
         * type = 2 : input is product type
         */
        /*        [Authorize(Roles = "Admin,Editor")]
        */
        public async Task<IActionResult> SearchProductsStaff(string input,string type)
            {
            List<Product> products = new List<Product>();
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
                        products =  _context.Product.Where(a => a.ProductName.Contains(input)).ToList();
                        }
                        else
                        {
                            products =  _context.Product.ToList();
                        }
                        break;
                    }
                case "1":
                    {
                        double price = Double.Parse(input);
                        products = _context.Product.Where(a => a.Price.Equals(price)).ToList();
                        break;
                    }
                case "2":
                    {
                        int theType = Int32.Parse(input);
                         products =  _context.Product.Where(a => a.CategoryId.Equals(theType)).ToList();
                        break;
                    }
                                }
            List<string> pathes = new List<string>();
            foreach (var pro in products)
            {
                Image image = _context.Image.Where(a => a.ProductId.Equals(pro.Id)).FirstOrDefault();
                if (image != null)
                {
                    pathes.Add(image.imagePath);
                }
            
            ViewData["pathes"] = pathes;

            }
            if(User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                return PartialView("SearchProductsStaff", products);
            }
            else
            {
                return PartialView("SearchProductsClient", products);

            }
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
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Editor")]
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
