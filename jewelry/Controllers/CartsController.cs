using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jewelry.Data;
using jewelry.Models;

namespace jewelry.Controllers
{
    public class CartsController : Controller
    {
        private readonly jewelryContext _context;

        public CartsController(jewelryContext context)
        {
            _context = context;
        }

        // GET: Carts
        public IActionResult MyCart(int? id)
        {
            if (id != null)
            {
                double totalPrice = 0;
                List<double> prices = new List<double>();
                List<ProductCart> cart =  _context.ProductCart.Where(a => a.CartId.Equals(id)).ToList();
                List<string> imagePathes = new List<string>();
                string imagePath = null;
                foreach(var productCart in cart)
                {
                    Image image = _context.Image.Where(a => a.ProductId.Equals(productCart.ProductId)).FirstOrDefault();
                    if (image != null)
                    {
                        imagePath = image.imagePath;
                    }
                    imagePathes.Add(imagePath);
                    Product product= _context.Product.Find(productCart.ProductId);
                    if (product == null) //אם מחקו את המוצר תסיר אותו מהעגלה גם ואז תקרא לפונצקיה מחדש
                    {
                        _context.ProductCart.Remove(productCart);
                        _context.SaveChangesAsync();
                        return (MyCart(id));
                    }
                    double price = product.Price;

                    prices.Add(price);
                    totalPrice += price*productCart.Quantity;
                }
                Cart mycart = _context.Cart.Find(id);
                if (mycart != null)
                {
                    mycart.TotalPrice = totalPrice;
                }
                _context.SaveChangesAsync();
                ViewData["totalPrice"] = totalPrice;
                ViewData["images"] = imagePathes;
                Tuple<List<ProductCart>, List<double>> tuple = new Tuple<List<ProductCart>, List<double>>(cart, prices);
                return PartialView(tuple);
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }


        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cart.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TotalPrice,LastUpdate")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TotalPrice,LastUpdate")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
