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
        public int AddressId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }
        
        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string Payment { get; set; } //paypal,visa,mastercard.....

        public List<ProductCart> OrderProducts { get; set; }

        public Boolean Sended { get; set; }//

        //payment implementation

    }
}
