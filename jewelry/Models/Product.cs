using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models

{
    public class Product
    {
        public enum ProductType
        {
            Necklace,
            Ring,
            Bracelet,
            Earrings
        }
        
        public int  Id { get; set; }

        [Required]

        public string ProductName { get; set; }

        [Required]

        public double Price { get; set; }

        [Required]

        public string Description { get; set; }

        [Required]

        public ProductType Type { get; set; }

        public int Discount { get; set; }

        public float RateSum { get; set; } = 5;

        public int Rates { get; set; } = 1;

        public List<User> Users { get; set; }

        public int Orders { get; set; }

        public int StoreQuantity { get; set; }
        public List<string> ImagesLinks { get; set; }
    }
   
}
