using jewelry.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace jewelry.Controllers
{
    public class AboutUsController : Controller
    {

        private readonly jewelryContext _context;

        public AboutUsController(jewelryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
