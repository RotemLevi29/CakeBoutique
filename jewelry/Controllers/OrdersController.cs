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
    public class OrdersController : Controller
    {
        private readonly jewelryContext _context;

        public OrdersController(jewelryContext context)
        {
            _context = context;
        }

         [HttpPost]
/*         [ValidateAntiForgeryToken]
*/         public IActionResult OrderForm([Bind("Id,Date,TotalPrice,Payment,Sended")] Order order,[Bind("PhoneNumber,State,City,Street,HouseNumber,ApartmentNumber,PostalCode")] Address address)
         {

             order.Sended = false;
             order.UserId = Int32.Parse(User.Claims.Where(c => c.Type.Equals("UserId")).Select(c => c.Value).SingleOrDefault());
             int cartid = Int32.Parse(User.Claims.Where(c => c.Type.Equals("cartId")).Select(c => c.Value).SingleOrDefault());
             order.Date = DateTime.Now;
             order.TotalPrice = _context.Cart.Find(cartid).TotalPrice;
             _context.Address.Add(address);
             _context.SaveChanges();
             order.AddressId = address.Id;

             if (ModelState.IsValid)
             {
                 order.AddressId = address.Id;
                 _context.Order.Add(order);
                 _context.SaveChanges();
                //clean cart await
                List<ProductCart> productCarts = _context.ProductCart.Where(a => a.CartId.Equals(cartid)).ToList();
                if(productCarts != null)
                {
                    foreach(ProductCart productcart in productCarts)
                    {
                        Product product = _context.Product.Find(productcart.ProductId);
                        if (product != null)
                        {
                            product.StoreQuantity -= 1;
                        }
                        _context.ProductCart.Remove(productcart);
                    }
                }
                Cart cart = _context.Cart.Find(cartid);
                if (cart != null)
                {
                    cart.TotalPrice = 0;
                }
                _context.SaveChanges();
                ViewData["orderid"] = order.Id;
                ViewData["arrivalDate"] = DateTime.Now.AddDays(7).Date;
                return PartialView("OrderDone");

             }
             return PartialView();
         }




        //OrderDone Get
        public IActionResult OrderDone(int id)
        {
            ViewData["orderid"] = id;
            ViewData["arrivalDate"] = DateTime.Now.AddDays(7).Date;
            return PartialView();
        }

        public IActionResult r( )
        {
          
            return PartialView();
        }

        //order get:
        [HttpGet]
        public IActionResult OrderForm(int total, int cartid)
        {
            //finding all the product, if the quantity are different than theres missing product
            if (cartid != null)
            {
                int quantity = 0;
                double totalPrice = 0;
                List<ProductCart> cart = _context.ProductCart.Where(a => a.CartId.Equals(cartid)).ToList();
                quantity = cart.Count();
                if (quantity != total || quantity == 0) // אם שינוי את הכמות תוך כדי
                {
                    return View("MyCart", "Carts");
                }
                foreach (var productCart in cart)
                {
                    Product product = _context.Product.Find(productCart.ProductId);

                    if (product == null)//אם מחקו את המוצר תוך כדי
                    {
                      return View("MyCart", "Carts");
                     }
                    
                    double price = product.Price;
                    totalPrice += price * productCart.Quantity;

                }
                if (totalPrice == 0)
                {
                    return View("MyCart", "Carts");
                }
                _context.Cart.Find(cartid).TotalPrice = totalPrice;
                _context.SaveChangesAsync();
                ViewData["totalPrice"] = totalPrice;
                ViewData["productCartList"] = cart;
                return PartialView();
                //אחר כך צריך לעשות creat order ושם להוריד את המלאי
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }

        }

        public IActionResult facebook()
        {
            return View();
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,TotalPrice,Payment,Sended")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }



        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,TotalPrice,Payment,Sended")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
