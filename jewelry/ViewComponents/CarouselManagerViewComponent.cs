using jewelry.Data;
using jewelry.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace jewelry.ViewComponents
{
    public class CarouselManagerViewComponent : ViewComponent 
    {
        private readonly jewelryContext _context;

        public CarouselManagerViewComponent(jewelryContext context)
        {
            _context = context;
        }

      
        public async Task<IViewComponentResult> InvokeAsync()
        {
          
            return View( _context.CarouselImage.ToList());
        }
    }
}
