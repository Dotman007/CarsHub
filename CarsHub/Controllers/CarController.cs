using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarsHub.Models;
using System.IO;
using System.Text;

namespace CarsHub.Controllers
{
    public class CarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        
        // GET: /Car/
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.Brand).Include(c => c.Dealer);
            return View(cars.ToList());
        }

        public ActionResult PostedCar()
        {
            int id = (int)(_context.Session["Dealer_Id"]);
            var postedCar = db.Cars.Include(deal => deal.Dealer).Where(deal => deal.DealerId == id).ToList();
            return View(postedCar);
        }

        public ActionResult OrderedCar()
        {
            int id = (int)(_context.Session["Dealer_Id"]);
            var orderedCar = db.Cars.Include(deal => deal.Dealer).Include(deal=>deal.Customer).Where(deal => deal.DealerId == id && deal.Book == true).ToList();
            return View(orderedCar);
        }


        public ActionResult MyBookedCar()
        {
            int id = (int)(_context.Session["Customer_Id"]);
            var bookedcars = db.Cars.Include(c => c.Dealer).Include(c => c.Customer).Where(c => c.CustomerId == id && c.Book == true).ToList();
            return View(bookedcars);
        }
        public ActionResult CarDetails(int id)
        {
            Car car = db.Cars.Find(id);
            return View(car);
        }

        public ActionResult ViewCars()
        {
           
            var cars = db.Cars.Include(deal => deal.Dealer).Where(deal => deal.Book != true ).OrderByDescending(deal=>deal.ActualTime);
            return View(cars.ToList());
        }


        public ActionResult OrderCar(int? id)
        {
            OrderActivity oa = new OrderActivity();
            Customer cus = new Customer();
            int custId = (int)(_context.Session["Customer_Id"]);
            
            Car car = db.Cars.Find(id);
            if (car.Book == true)
            {
                @ViewBag.Output = "Car Booked Already!!";
            }
            if (car.Book != true)
            {
                
                car.Book = true;
                car.CustomerId = custId;
                car.BookedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("OrderSuccess");
            }

            return View();
        }

        public ViewResult OrderSuccess()
        {
            return View();
        }
        // GET: /Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: /Car/Create
        public ActionResult Create()
        {
            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name");

            return View();
        }

        // POST: /Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,Brand_Id,CarModel,Milage,Year,Condition,FrontView,Price,DealerId,ActualTime,BookedDate")] Car car, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (_context.Session["Dealer_Id"] == null)
                {
                    return RedirectToAction("DealerLogin", "Dealer");
                }

                if (file != null)
                {

                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("/Uploads"), pic);
                    file.SaveAs(path);
                    car.FrontView = file.FileName;
                }
               
                int id = (int)(_context.Session["Dealer_Id"]);
                car.DealerId = id;
                car.ActualTime = DateTime.Now;
                car.BookedDate = null;
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("CarSuccess");
            }

            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name", car.Brand_Id);

            return View(car);
        }

        public ActionResult UploadCar()
        {
            
            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name");
            return View();
        }
    

        [HttpPost]
        public ActionResult UploadCar(Car car)

        {
            for(int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase postedFile = Request.Files[i];
                if(postedFile.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(Server.MapPath("/Uploads")+filename);
                }

            
                
                
            }
            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name", car.Brand_Id);
            return View();
        }

        

        public ActionResult Order()
        {


            return View();
        }

        // POST: /Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order([Bind(Include = "CarId,DealerId,MobileNo,Email,HomeAddress")] Car car,string phone,string mail,string address)
        {
            if (ModelState.IsValid)
            {


                int id = (int)(_context.Session["Dealer_Id"]);
                int id2 = (int)(_context.Session["Customer_Id"]);

                //Customer cust = new Customer();
                //car.CarDetails = car.Brand.Brand_Name + " "  + car.CarModel + " " +  car.Milage;
                car.DealerId = id;
                car.CustomerId = id2;
                //car.Customer.Telephone = phone;
                //car.Customer.UserName = name;
                car.HomeAddress = address;
                car.MobileNo = phone;
                car.Email = mail;
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("OrderSuccess");
            }



            return View(car);
        }
        public ViewResult CarSuccess()
        {
            return View();

        }




        // GET: /Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name", car.Brand_Id);

            return View(car);
        }

        // POST: /Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarId,Brand_Id,CarModel,Milage,Year,Condition,FrontView,Price,DealerId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brand_Id = new SelectList(db.Brands, "Brand_Id", "Brand_Name", car.Brand_Id);
            //ViewBag.DealerId = new SelectList(db.Dealers, "DealerId", "CompanyName", car.DealerId);
            return View(car);
        }

        // GET: /Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: /Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
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
