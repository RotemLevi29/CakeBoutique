using CakeBoutique.Data;
using CakeBoutique.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace cakeBotique.ViewComponents
{
    public class ButtomLayoutViewComponent : ViewComponent 
    {

        public ButtomLayoutViewComponent(CakeBoutiqueContext context)
        {
          
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View();
        }
    }
}
