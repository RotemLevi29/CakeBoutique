using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CakeBoutique.Models;

namespace CakeBoutique.Data
{
    public class CakeBoutiqueContext : DbContext
    {

        public CakeBoutiqueContext(DbContextOptions<CakeBoutiqueContext> options)
            : base(options)
        {
        }

        public DbSet<CakeBoutique.Models.Product> Product { get; set; }

        public DbSet<CakeBoutique.Models.Image> Image { get; set; }

        public DbSet<CakeBoutique.Models.User> User { get; set; }

        public DbSet<CakeBoutique.Models.Cart> Cart { get; set; }

        public DbSet<CakeBoutique.Models.CarouselImage> CarouselImage { get; set; }

        public DbSet<CakeBoutique.Models.Category> Category { get; set; }

        public DbSet<CakeBoutique.Models.ProductCart> ProductCart { get; set; }

        public DbSet<CakeBoutique.Models.Address> Address { get; set; }

        public DbSet<CakeBoutique.Models.Order> Order { get; set; }

    }
}
