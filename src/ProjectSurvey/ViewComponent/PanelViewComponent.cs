using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSurvey.Models;

namespace ProjectSurvey.ViewComponent
{
    public class PanelViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent            
    {
          public async Task<IViewComponentResult> InvokeAsync(DbSet<Sphere> Spheres)     
        {
            return View(Spheres.ToList());
        }
    }
}
