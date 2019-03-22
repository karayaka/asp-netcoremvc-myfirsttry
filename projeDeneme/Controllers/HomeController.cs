using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using projeDeneme.Models;

namespace projeDeneme.Controllers
{
    public class HomeController : Controller
    {
        private ModelmysqlContext _context;
        //ModelmysqlContext db = new ModelmysqlContext();
        public HomeController(ModelmysqlContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var model = _context.Modelmysqls.ToList();

            return View(model);
        }
        public ActionResult Index2(string id)
        {
            var user = HttpContext.User.Identity;

            return View(user);
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