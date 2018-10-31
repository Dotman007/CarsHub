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
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;


        //Add A New Admin
        [HttpGet]
        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(Admin admin)
        {
            
            try
            {
                if(_context.Session["Admin_Id"] != null)
                {
                    @ViewBag.Success = "Admin user was created successfully";
                    db.Admins.Add(admin);
                    admin.Username = GenerateAdminReg(admin.AdminId);
                    admin.Password = GenerateAdminPassword();
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            catch (Exception)
            {

                @ViewBag.Error = "Admin user creation Failed";
            }
            return View();
        }

        //Admin Login
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AdminLogin(string username, string password)
        {
           if(username == "")
           {
               ViewBag.EmptyUserName = "Username is Required";
               return View("AdminLogin");
           }
           if(password == "")
           {
               ViewBag.EmptyPassword = "Password is Required";
               return View("AdminLogin");
           }
           if(username =="" && password == "")
           {
               ViewBag.EmptyUserName = "Username is Required";
               ViewBag.EmptyPassword = "Password is Required";
               return View("AdminLogin");
           }
           var adminer = db.Admins.SingleOrDefault(a => a.Username == username && a.Password == password);
           if(adminer != null)
           {
               ViewBag.EmptyUserName = null;
               ViewBag.EmptyPassword = null;
               ViewBag.NotInExistence = null;

               _context.Session["Full_Name"] = adminer.FullName;
               _context.Session["User_Name"] = adminer.Username;
               _context.Session["Admin_Id"] = adminer.AdminId;
               _context.Session["Email_Ad"] = adminer.Email;

               return RedirectToAction("AdminDashboard", "Admin");
           }
           
           _context.Session["Full_Name"] = null;
           _context.Session["User_Name"] = null;
           _context.Session["Admin_Id"] = null;
           _context.Session["Email_Ad"] = null;
           ViewBag.NotInExistence = "The Username and Password does not exist.";
           return View("AdminLogin");
        }


        public ActionResult AdminDashboard()
        {
            if(_context.Session["Admin_Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            int id = (int)(_context.Session["Admin_Id"]);
            Admin admin = db.Admins.Find(id);
            return View(admin);
        }


        //Generate Registration Number
        public string GenerateAdminReg(int adminId)
        {
            Random rand = new Random();
            var getNumber = rand.Next(10000, 20000);
            var genPass = "Admin" + getNumber.ToString();
            return genPass;
        }



        //Generate Password

        public string GenerateAdminPassword()
        {
            var allowChars = "";
            const int length = 8;
            allowChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowChars += "0,1,2,3,4,5,6,7,8,9";
            var arr = allowChars.Split(',');
            var password = "";
            var randd = new Random();
            for(int i = 0; i < length; i++)
            {
                var temp = arr[randd.Next(0, arr.Length)];
                password += temp;
            }
            return password;

        }

        public ActionResult ViewCustomers()
        {
            var customers = db.Customer.ToList();
            return View(customers);
        }



        public ActionResult ViewDealers()
        {
            var dealers = db.Dealers.ToList();
            return View(dealers);
        }

        // GET: /Admin/
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: /Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AdminId,Username,Password,Passport")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: /Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: /Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AdminId,Username,Password,Passport")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: /Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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
