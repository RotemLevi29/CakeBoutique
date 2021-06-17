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
            Earrings,
            Men
        }

        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(maximumLength: 38)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Range(0, 100)]
        public int Discount { get; set; } = 0;//precent


        //The rate will be = RateSum/Rates
        public float RateSum { get; set; } = 0;

        public int Rates { get; set; } = 0;
        

        public int Orders { get; set; } = 0;//++1 for each oreder with this product


        public int StoreQuantity { get; set; } = 100;


        //product attributes
        [Display(Name ="Name option")]
        public int NameOption { get; set; }//2=option for letter,1=option for word, 0=reguler, no option for word

    }
   
}
