using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Carousel
    {
        public int Id { get; set; }
        public int ImageNumber { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Image> Carouselmages { get; set; }
    }
}
