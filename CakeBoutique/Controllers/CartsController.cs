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
    public class CartsController : Controller
    {
        private readonly CakeBoutiqueContext _context;

        public CartsController(CakeBoutiqueContext context)
        {
            _context = context;
        }

        // GET: Carts
        [Authorize]
        public IActionResult MyCart(int? id)
        {
            if (TempData["cartError"]!=null)
            {
                ViewData["cartError"] = "error";
            }
            if (id != null)
            {
                double totalPrice = 0;
                Cart mycart = (_context.Cart.Include(a=>a.ProductCartId)).First(a=>a.Id.Equals(id));
                if(mycart == null)
                {
                    return NotFound();
                }
                List<double> prices = new List<double>();
                List<ProductCart> productcarts = mycart.ProductCartId;
                List<Image> images = new List<Image>();
                foreach(var productCart in productcarts)
                {
                    Image image = _context.Image.Where(a => a.ProductId.Equals(productCart.ProductId)).FirstOrDefault();
                    if (image != null)
                    {
                        images.Add(image);
                    }
                    Product product= _context.Product.Find(productCart.ProductId);
                    if (product == null)
                    {
                        _context.ProductCart.Remove(productCart);
                        _context.SaveChanges();
                        return (MyCart(id));
                    }
                    double price = product.Price;

                    prices.Add(price);
                    totalPrice += price*productCart.Quantity;
                }
                if (mycart != null)
                {
                    mycart.TotalPrice = totalPrice;
                }
                _context.SaveChangesAsync();
                ViewData["totalPrice"] = totalPrice;
                ViewData["images"] = images;
                Tuple<List<ProductCart>, List<double>> tuple = new Tuple<List<ProductCart>, List<double>>(productcarts, prices);
                return PartialView(tuple);
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
    }
}
