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
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCart.ToListAsync());
        }

        // GET: ProductCarts/Details/5
        [Authorize(Roles = "Admin,Editor")]
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
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Editor")]
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


        public void RemoveFromCart(int? id)
        {
            if (id != null)
            {
                var productToRemove = _context.ProductCart.Find(id);
                if(productToRemove != null)
                {
                    if(productToRemove.Quantity==1)
                    {
                        _context.Remove(productToRemove);
                    }
                    else
                    {
                        productToRemove.Quantity -= 1;
                    }
                    _context.SaveChanges();

                }
            }
        }

/*        [Authorize]
*/        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> AddToCart(int productId, string productName, string input,string url)
        {
            // if the product cart exist in this cart +1
            if (User.Identity.IsAuthenticated)
            {
                int userid = Int32.Parse(User.Claims.SingleOrDefault(c => c.Type == "UserId").Value);
                User user = _context.User.Find(userid);
                int cartId = user.CartId;
                Cart cartClaim = _context.Cart.Find(cartId);

                if (cartClaim == null)//זה אומר שהעגלה לא קיימת(לא אמור לקרות)
                {
                    Cart newCart = new Cart();
                    newCart.UserId = userid;
                    _context.Cart.Add(newCart);
                    _context.SaveChanges();
                    cartId = newCart.Id;
                    user.CartId = cartId;
                    _context.SaveChanges();
                    //אם זה קרה, נחזיר את הלקוח להתחבר מחדש כדי שהעגלה תתעדכן כמו שצריך, הפעולה לא תוסיף עדיין את המוצר
                    return "NotLogin";
                }
                var productExist = (from u in _context.ProductCart
                                    where u.ProductId == productId &&
                                    u.CartId == cartId &&
                                    u.CustumName == input
                                    select u).FirstOrDefault();
                if (productExist != null && _context.Product.Find(productId).StoreQuantity > 0)
                {
                    productExist.Quantity += 1;
                    await _context.SaveChangesAsync();
                    return "Success";
                }

                Product product = _context.Product.Find(productId);
                if (product != null)
                {
                    if (product.StoreQuantity > 0)
                    {
                        ProductCart productcart = new ProductCart();
                        productcart.CustumName = input;
                        productcart.ProductId = productId;
                        productcart.Quantity = 1;
                        productcart.ProductName = productName;
                        productcart.CartId = cartId;
                        _context.Add(productcart);
                        await _context.SaveChangesAsync();
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
                else
                {
                    return "Error";
                }
            }
            else
            { return "NotLogin"; }            
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
