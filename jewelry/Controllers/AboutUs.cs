using jewelry.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace jewelry.Controllers
{
    public class AboutUs : Controller
    {

        private readonly jewelryContext _context;

        public AboutUs(jewelryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
