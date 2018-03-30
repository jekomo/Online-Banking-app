using ABCBankApps.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABCBankApps.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        ABCBank dbContext = new ABCBank();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NewCustomer()
        {

            return View();
        }

        [HttpPost]
        public ActionResult NewCustomer(CustomersAcct cs)
        {
            ViewBag.ValidResult = "Null";
            if (ModelState.IsValid)
            {
                dbContext.CustomersAccts.Create();
                cs.dDate = DateTime.Now;
                dbContext.CustomersAccts.Add(cs);
                dbContext.SaveChanges();
                ViewBag.ValidResult = "Success";
                ModelState.Clear();
                ModelState.AddModelError("", "Customer Added Succesfully !");
            }
            return View();
        }

        [HttpGet]
        public ActionResult CheckBalance()
        {
            return View();

        }
        [HttpPost]
        public ActionResult checkBalance(string iAcNo)
        {
            ViewBag.ValidResult = "Null";
            var CusAcctNoFind = dbContext.CustomersAccts.Find(Convert.ToInt64(iAcNo));

            if (CusAcctNoFind == null)
            {
                ViewBag.ValidResult = "NoAcctNo";
                ModelState.AddModelError("", "Invalid account Number");
            }
            else
            {

                ModelState.AddModelError("", "Account Number is Valid");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ViewCheckBal", CusAcctNoFind);
            }
            return View(CusAcctNoFind);
        }

        [HttpGet]
        public ActionResult newTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult newTransaction(Transaction newTrans)
        {
            ViewBag.ValidResult = "Null";
            if (ModelState.IsValid)
            {

                var CusAcctNoFind = dbContext.CustomersAccts.Find(Convert.ToInt64(newTrans.iacno));

                if (CusAcctNoFind == null)
                {
                    ViewBag.ValidResult = "NoAcctNo";
                    ModelState.AddModelError("", "Invalid account Number");
                }
                else
                {

                    newTrans.TransDate = DateTime.Now;
                    dbContext.Transactions.Add(newTrans);
                    dbContext.SaveChanges();
                    ViewBag.ValidResult = "Success";
                    ModelState.Clear();
                    ModelState.AddModelError("", "Transaction was Sucessfully !!");
                }

            }
            return View();

        }


        public ActionResult Edit(int id)
        {

            var CusEdit = dbContext.CustomersAccts.Find(id);
            if (CusEdit == null)
            {
                return HttpNotFound();
            }
            return View(CusEdit);
        }

        [HttpPost]
        public ActionResult Edit(CustomersAcct cs)
        {

            if (ModelState.IsValid)
            {
                dbContext.Entry(cs).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(cs);
            }
        }
    }

}
