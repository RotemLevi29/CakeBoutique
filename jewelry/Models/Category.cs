using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Category Name")]

        public string CategoryName { get; set; }

        public List<Product> products { get; set; }

        public byte[] Image { get; set; }

        public int Interest { get; set; } = 0;
    }
}
