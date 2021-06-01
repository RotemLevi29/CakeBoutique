using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Order
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public User User { get; set; }
        
        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string Payment { get; set; } //paypal,visa,mastercard.....

        [Required]
        public List<ProductCart> OrderProducts { get; set; }

        //payment implementation

    }
}
