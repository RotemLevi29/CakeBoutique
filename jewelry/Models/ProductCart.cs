using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/**
 * This product cart holding product ID,
 * in addition it's holding the attributes as size color and price.
 * it will also has a field for which userCart it belongs as CartId,
 */
namespace jewelry.Models
{
    public class ProductCart
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [MaxLength(10)]
        public string CustumName { get; set; } = null;

        public int CartId { get; set; }

    }
}
