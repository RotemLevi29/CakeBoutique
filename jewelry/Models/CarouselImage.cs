using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class CarouselImage
    {
        public int  Id { get; set; }
        public int? Width { get; set; } = 0;
        public int? Height { get; set; } = 0;   
        public int CarImageId { get; set; } //Image object
    }
}
