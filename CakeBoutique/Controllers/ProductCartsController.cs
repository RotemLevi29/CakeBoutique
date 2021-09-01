using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CakeBoutique.Data;
using CakeBoutique.Models;
using Microsoft.AspNetCore.Authorization;

namespace CakeBoutique.Controllers
{
    public class ProductCartsController : Controller
    {
        private readonly CakeBoutiqueContext _context;

        public ProductCartsController(CakeBoutiqueContext context)
        {
            _context = context;
        }

        // GET: ProductCarts
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCart.ToListAsync());
        }

        public async Task<string> changeQuantity(int ? id, int quantity)
        {
            if (id != null)
            {
                var prodcutToChange = _context.ProductCart.Find(id);
                if (prodcutToChange != null)
                //check if the products exist.....
                {
                    
                    var pro = _context.Product.Find(prodcutToChange.ProductId);
                    Cart cart = _context.Cart.Find(prodcutToChange.CartId);
                    var oldQuantity = prodcutToChange.Quantity;
                    if (pro != null)
                    {
                        if (pro.StoreQuantity - quantity >= 0)
                        {
                            prodcutToChange.Quantity = quantity;

                        }
                        else
                        {
                            return "quantityError";

                        }
                        if (quantity == 0)
                        {
                            _context.ProductCart.Remove(prodcutToChange);
                            _context.SaveChanges();
                            return "zeroQuantity";
                            
                        }
                        double newTotal = cart.TotalPrice + (pro.Price * (quantity-oldQuantity));
                        cart.TotalPrice = newTotal;
                        await _context.SaveChangesAsync();
                        return pro.Price.ToString() + ',' + newTotal.ToString() + ',' + (quantity*pro.Price).ToString() +','+ quantity.ToString();
                    }
                }
            }
            return "error";
        }

        
        /**
         * This function gets custom product and details, if the product is valid
         * so the quantity exist it adding the custom product to the cart.
         */
        [HttpPost]
        public async Task<string> AddToCart(int productId, string productName, string input,string url,int quantity)
        {
            if (quantity < 0)
            {
                return "toomany";
            }
            // if the product cart exist in this cart +1
            if (User.Identity.IsAuthenticated)
            {
                int userid = Int32.Parse(User.Claims.SingleOrDefault(c => c.Type == "UserId").Value);
                User user = _context.User.Find(userid);
                if (user == null)
                {
                    return "Error";
                }
                int cartId = user.CartId;
                Cart cartClaim = _context.Cart.Find(cartId);

                if (cartClaim == null)//if the cart doesn't exist, creating new cart fot this user
                {
                    Cart newCart = new Cart();
                    newCart.UserId = userid;
                    _context.Cart.Add(newCart);
                    _context.SaveChanges();
                    cartId = newCart.Id;
                    user.CartId = cartId;
                    _context.SaveChanges();
                   //created the cart id, telling the user to relogin
                    return "NotLogin";
                }
                var productExist = (from u in _context.ProductCart
                                    where u.ProductId == productId &&
                                    u.CartId == cartId &&
                                    u.CustumName == input
                                    select u).FirstOrDefault();
                if (productExist != null) 
                {
                    var prod = _context.Product.Find(productId);
                    if (prod != null && prod.StoreQuantity - quantity >= 0)
                    {
                        productExist.Quantity += quantity;
                        await _context.SaveChangesAsync();
                        return "Success";

                    }
                    else return "toomany";
                }
               
                Product product = _context.Product.Find(productId);
                if (product != null)
                {
                    if (product.StoreQuantity -quantity >= 0)
                    {
                        ProductCart productcart = new ProductCart();
                        productcart.CustumName = input;
                        productcart.ProductId = productId;
                        productcart.Quantity = quantity;
                        productcart.ProductName = productName;
                        productcart.CartId = cartId;
                        _context.Add(productcart);
                        _context.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        return "toomany";
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
