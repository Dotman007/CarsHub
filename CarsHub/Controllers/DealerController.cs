using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsHub.Models;
using System.Web.Helpers;

namespace CarsHub.Controllers
{
    public class DealerController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        // GET: /Dealer/
        public ActionResult Index()
        {
            return View(db.Dealers.ToList());
        }

        // GET: /Dealer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }


                   
        // GET: /Dealer/Create
        public ActionResult Create()
        {
            return View();
           
        }

        // POST: /Dealer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DealerId,CompanyName,Country,Province,CompanyAddress,EmailAddress,OfficePhoneNo,RegistrationNo,Password")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                
                
                dealer.RegistrationNo = GenerateId(dealer.DealerId);
                dealer.Password = GeneratePassword();
                db.Dealers.Add(dealer);
                db.SaveChanges();
                //WebMail.SmtpServer = "smtp-mail.outlook.com";
                //WebMail.SmtpPort = 587;
                //WebMail.SmtpUseDefaultCredentials = true;
                //WebMail.EnableSsl = true;
                //WebMail.UserName = "e-lautech@outlook.com";
                //WebMail.Password = "iyaniwura1";
                //WebMail.From = "e-lautech@outlook.com";
                //WebMail.Send(to: dealer.EmailAddress, subject: "Confirmation Email",
                //    body: "Hello " + " " + dealer.CompanyName + " "
                //    + "<br />" + "Your Registration Number is :" + " " +
                //    dealer.RegistrationNo + "<br /> " + "Your Password is : "+dealer.Password + "<br />", isBodyHtml: true);
                //ViewBag.Message = "Dealer Registered Successfully";
                return RedirectToAction("Success");
                

            }
             
            return View(dealer);
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

                _context.Session["RegistrationNo"] =dealer.RegistrationNo ;
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
        
        public string GenerateId(int dealerId)
        {
                       
            var rNum = new Random();
            var randId = rNum.Next(1000000, 9000000);
            var studId = "CH" + DateTime.Now.Year + randId;
            return studId.ToString();
        }

        public string GeneratePassword()
        {
            var allowedChas = "";
            const int length = 8;
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

        public ViewResult Success()
        {
            return View();
        }


        public ActionResult ChangePassword(int? id)
        {
            if (_context.Session["Dealer_Id"] == null)
            {
                return RedirectToAction("DealerLogin","Dealer");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            
            return View(dealer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "DealerId,CompanyName,Country,Province,CompanyAddress,EmailAddress,OfficePhoneNo,RegistrationNo,Password")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(dealer);
        }
        // GET: /Dealer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: /Dealer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DealerId,CompanyName,Country,Province,CompanyAddress,EmailAddress,OfficePhoneNo")] Dealer dealer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }

        // GET: /Dealer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: /Dealer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dealer dealer = db.Dealers.Find(id);
            db.Dealers.Remove(dealer);
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
