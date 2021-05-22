using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class ProductCart
    {
        public int Id { get; set; }
        public enum ProductColor
        {
            Gold_18k,
            Gold_14k,
            White_Gold,
            Rose,
            Silver
        }

        [Required]
        public double size { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public ProductColor Color { get; set; }

    }
}
