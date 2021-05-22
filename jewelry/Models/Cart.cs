using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Cart
    {
        public enum ProductColor
        {
            Gold_18k,
            Gold_14k,
            White_Gold,
            Rose,
            Silver
        }
        public int Id { get; set; }
        public List<Tuple<Product,ProductColor>> Products { get; set; }
        public User User { get; set; }
        public double Price { get; set; }
    }
}
