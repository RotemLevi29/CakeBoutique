using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public List<Product> products { get; set; }

        public int ImageId { get; set; }

        public int Interest { get; set; } = 0;
    }
}
