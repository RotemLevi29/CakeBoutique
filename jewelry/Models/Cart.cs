using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public List<ProductCart> ProductCart { get; set; }
        [Required]
        public User User { get; set; }

        public double TotalPrice { get; set; }
    }
}
