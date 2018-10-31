using CarsHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarsHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult DealerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DealerLogin(string regNo, string pword)
        {
            if (regNo == "" && pword == "")
            {
                ViewBag.RegNo = "Registration No required";
                ViewBag.Pword = "Password required";

                return View("DealerLogin");
            }
            if (regNo == "")
            {
                ViewBag.RegNo = "Registration No required";

                return View("DealerLogin");
            }
            if (pword == "")
            {
                ViewBag.Pword = "Password required";

                return View("DealerLogin");
            }
            var dealer = db.Dealers.SingleOrDefault(u => u.RegistrationNo == regNo && u.Password == pword);

            if (dealer != null)
            {
                ViewBag.RegNo = null;
                ViewBag.Pword = null;
                ViewBag.DealerNotExist = null;

                _context.Session["RegistrationNo"] = dealer.RegistrationNo;
                _context.Session["Dealer_Id"] = dealer.DealerId;
                _context.Session["Dealer_Name"] = dealer.CompanyName;

                return RedirectToAction("Dashboard", "Dealer");
            }

            _context.Session["RegistrationNo"] = null;
            _context.Session["Dealer_Id"] = null;
            _context.Session["Dealer_Name"] = null;

            ViewBag.DealerNotExist = "Invalid MatricNo/Password";

            return View("DealerLogin");
        }

        public ActionResult Dashboard()
        {
            if (_context.Session["Dealer_Id"] == null)
            {
                return RedirectToAction("DealerLogin", "Dealer");
            }

            int id = (int)(_context.Session["Dealer_Id"]);
            var dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

    }
}