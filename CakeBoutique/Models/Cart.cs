using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/**
 *The cart class holding list of productCartId, the data is in 
 *the productCart table, foreach product cart is for one cart.
 *we saving here the total price and updating it every time there is 
 *a new productCart.
 *we also updating the date everytime there is a new product.
 *after a week we will clean the product cart.
 */
namespace CakeBoutique.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public List<ProductCart> ProductCartId { get; set; }
        
        //not rquiered because if the user doesnt registered
        //he also can have a cart but it won't be writing in data base.
        //it will be temporary Cart.
        public int UserId { get; set; }

        [Required]
        public double TotalPrice { get; set; } = 0;

        public DateTime LastUpdate { get; set; }//after week we will clean the cart
    }
}
