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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderForm([Bind("Id,Date,TotalPrice,Payment,Sended")] Order order,[Bind("PhoneNumber,State,City,Street,HouseNumber,ApartmentNumber,PostalCode")] Address address)
        {
            order.Sended = false;
            order.UserId = Int32.Parse(User.Claims.Where(c => c.Type.Equals("userId")).Select(c => c.Value).SingleOrDefault());


            int cartid = Int32.Parse(User.Claims.Where(c => c.Type.Equals("cartId")).Select(c => c.Value).SingleOrDefault());
            order.Date = DateTime.Now;
            order.TotalPrice = _context.Cart.Find(cartid).TotalPrice;
            if (ModelState.IsValid)
            {


                return PartialView("sda","asd");

            }
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
                if (quantity != total) // אם שינוי את הכמות תוך כדי
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
