using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsHub.Models;

namespace CarsHub.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        // GET: /Customer/
        public ActionResult Index()
        {
            return View(db.Customer.ToList());
        }


        public ActionResult Dashboard()
        {
            if (_context.Session["Customer_Id"] == null)
            {
                return RedirectToAction("CustomerLogin", "Customer");
            }

            int id = (int)(_context.Session["Customer_Id"]);
            var customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        
        public ActionResult CustomerLogin(string username, string password)
        {
            if (username == "" && password == "")
            {
                @ViewBag.userName = "User Name is Required";
                @ViewBag.PassWord = "Password is Required";
                return RedirectToAction("CustomerLogin");
            }
            if (username == "")
            {
                @ViewBag.userName = "User Name is Required";
                return RedirectToAction("CustomerLogin");
            }
            if (password == "")
            {
                @ViewBag.PassWord = "Password is Required";
                return RedirectToAction("CustomerLogin");
            }
            var customer = db.Customer.SingleOrDefault(c => c.UserName == username && c.Password == password);
            if (customer != null)
            {
                @ViewBag.userName = null;
                @ViewBag.PassWord = null;
                @ViewBag.CustomerNotExist = null;

                _context.Session["Username"] = customer.UserName;
                _context.Session["Customer_Id"] = customer.CustomerId;
                _context.Session["Email_Address"] = customer.Email;

                return RedirectToAction("ViewCars", "Car");
            }

            _context.Session["Username"] = null;
            _context.Session["Customer_Id"] = null;
            _context.Session["Email_Address"] = null;

            ViewBag.DealerNotExist = "Invalid MatricNo/Password";

            return View("CustomerLogin");
        }

        // GET: /Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Customer/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countrys, "CountryId", "CountryName");
          
            return View();
        }

        // POST: /Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,UserName,Password,Email,Telephone,Address,CountryId,StateId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Password = GeneratePass();
                //customer.UserName = GenerateUserId(customer.CustomerId);
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("CustomerSuccess");
            }
            ViewBag.CountryId = new SelectList(db.Countrys, "CountryId", "CountryName",customer.CustomerId);
            
            return View(customer);
        }

        public string GenerateUserId(int customerId)
        {

            var rNum = new Random();
            var randId = rNum.Next(100000, 900000);
            var studId = "CUST" + DateTime.Now.Year + randId;
            return studId.ToString();
        }

        //public JsonResult GetCountry()
        //{

        //    var countries = db.Countrys.ToList();
            
        //    return this.Json(countries, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult GetStates(int countryID)
        //{

        //    var states = db.States.Where(m => m.CountryId == countryID).ToList();

        //    return this.Json(states, JsonRequestBehavior.AllowGet);
        //}
        public ViewResult CustomerSuccess()
        {
            return View();
        }

        // GET: /Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,UserName,Password,Email,Telephone,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public string GeneratePass()
        {
            var allowedChas = "";
            const int length = 10;
            allowedChas += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowedChas += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowedChas += "0,1,2,3,4,5,6,7,8,9";
            var arr = allowedChas.Split(',');
            var password = "";
            var rand = new Random();
            for (int i = 0; i < length; i++)
            {
                var temp = arr[rand.Next(0, arr.Length)];
                password += temp;
            }
            return password;
        }

        // GET: /Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
