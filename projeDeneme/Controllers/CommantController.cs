using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace projeDeneme.Controllers
{
    public class CommantController : Controller
    {
        [Route("/error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}