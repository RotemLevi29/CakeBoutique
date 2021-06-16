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
        private List<string> _pathes;
        private readonly jewelryContext _context;

        public CarouselManagerViewComponent(jewelryContext context)
        {
            _context = context;
            getPathes();
        }

        private void getPathes()
        {
            _pathes = new List<string>();
            var CarouselImages = _context.CarouselImage.ToList();
            foreach (var carousel in CarouselImages)
            {
                _pathes.Add(_context.Image.Where(a => a.Id.Equals(carousel.CarImageId)).FirstOrDefault().imagePath);
            }
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           if(this._pathes.Count == 0)
            {
                getPathes();
            }
            return View(_pathes);
        }
    }
}
