using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.Models
{
    public class Image
    {
        public enum ImageType
        {
            Gold_18k,
            Gold_14k,
            White_Gold,
            Rose,
            Silver,
            Carousel,
            Profile,
            Category
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] image { get; set; }

        public int? ProductId { get; set; }

        public ImageType Type { get; set; }

    }
}
