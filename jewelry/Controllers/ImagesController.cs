using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jewelry.Data;
using jewelry.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace jewelry.Controllers
{
    public class ImagesController : Controller
    {
        private readonly jewelryContext _context;

        public ImagesController(jewelryContext context)
        {
            _context = context;
        }


        /**
         * This function using only by other entities,
         * because every image related to some model.
         */
        [Authorize(Roles = "Admin,Editor")]
        public async void regularDelete(int id)
        {
            var image = await _context.Image.FindAsync(id);
            if (image != null)
            {
                _context.Image.Remove(image);
            }
        }
        [Authorize(Roles = "Admin,Editor")]

        private bool ImageExists(int id)
        {
            return _context.Image.Any(e => e.Id == id);
        }
    }
}
