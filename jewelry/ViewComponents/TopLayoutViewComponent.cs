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
    public class TopLayoutViewComponent : ViewComponent 
    {

        public TopLayoutViewComponent(jewelryContext context)
        {
          
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View();
        }
    }
}
