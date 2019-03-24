using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projeDeneme.Identity;
using projeDeneme.Models;

namespace projeDeneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private ModelmysqlContext _context;
        //ModelmysqlContext db = new ModelmysqlContext();
        public HomeController(ModelmysqlContext context, UserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public ActionResult Index()
        {
            var model = _context.Modelmysqls.ToList();

            return View(model);
        }
        public ActionResult Index3()
        {
            var model = _context.Modelmysqls.ToList();

            return Json(model);
        }
        public ActionResult Index2()
        {
            //oturumdaki User Id:
            var user = _userManager.GetUserId(HttpContext.User);
            ViewBag.user = user;
            return View();
        }
        public ActionResult Yeni()
        {           

            return View();
        }
        [HttpPost]
        public ActionResult Yeni(Modelmysql modelmysql)
        {
            _context.Add(modelmysql);
            _context.SaveChanges();
            return Redirect("index");
        }
        public ActionResult Guncelle(Modelmysql modelmysql)
        {
            
            return View();
        }
    }
}