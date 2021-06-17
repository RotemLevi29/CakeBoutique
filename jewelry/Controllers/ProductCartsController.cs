using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Authorization;

namespace jewelry.Controllers
{
    public class ProductCartsController : Controller
    {
        private readonly jewelryContext _context;

        public ProductCartsController(jewelryContext context)
        {
            _context = context;
        }

        // GET: ProductCarts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCart.ToListAsync());
        }

        // GET: ProductCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCart = await _context.ProductCart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCart == null)
            {
                return NotFound();
            }

            return View(productCart);
        }   

        // GET: ProductCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,ProductId,ProductName,CustumName,CartId")] ProductCart productCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productCart);
        }

        //[Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<bool> AddToCart(int productId,string productName, string input,int cartId,string url)
        {

            if (User.Identity.IsAuthenticated )
            {
                ProductCart productcart = new ProductCart();
                productcart.CustumName = input;
                productcart.ProductId = productId;
                productcart.Quantity = 1;
                productcart.ProductName = productName;
                productcart.CartId = cartId;
                _context.Add(productcart);
                 await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
/*                return RedirectToAction("Login", "Users",new { ReturnUrl = url });
*/
            }
            
        }


        // GET: ProductCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCart = await _context.ProductCart.FindAsync(id);
            if (productCart == null)
            {
                return NotFound();
            }
            return View(productCart);
        }

        // POST: ProductCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,ProductId,ProductName,CustumName,CartId")] ProductCart productCart)
        {
            if (id != productCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCartExists(productCart.Id))
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
            return View(productCart);
        }

        // GET: ProductCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCart = await _context.ProductCart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productCart == null)
            {
                return NotFound();
            }

            return View(productCart);
        }

        // POST: ProductCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCart = await _context.ProductCart.FindAsync(id);
            _context.ProductCart.Remove(productCart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCartExists(int id)
        {
            return _context.ProductCart.Any(e => e.Id == id);
        }
    }
}
