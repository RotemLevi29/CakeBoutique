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
                        _context.SaveChanges();
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
    }
}
