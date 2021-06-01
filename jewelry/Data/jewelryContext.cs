using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using jewelry.Models;

namespace jewelry.Data
{
    public class jewelryContext : DbContext
    {
        public jewelryContext (DbContextOptions<jewelryContext> options)
            : base(options)
        {
        }

        public DbSet<jewelry.Models.Product> Product { get; set; }

        public DbSet<jewelry.Models.Image> Image { get; set; }
    }
}
