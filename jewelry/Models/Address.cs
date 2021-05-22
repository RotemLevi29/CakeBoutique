using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    //blah
    public class Address
    {
        //master changes
        public int  Id { get; set; }

        [DataType(DataType.PhoneNumber)]
        public long PhoneNumber { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string ApartmentNumber { get; set; }

        [Required]
        public int PostalCode { get; set; }
    }
}
