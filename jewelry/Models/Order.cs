using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Order
    {
        public int Id { get; set; }

        public Address Address { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }
        public double TotalPrice { get; set; }

        public List<ProductCart> OrderProducts { get; set; }

        //payment implementation

    }
}
