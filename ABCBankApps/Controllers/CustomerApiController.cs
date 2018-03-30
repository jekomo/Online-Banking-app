using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ABCBankApps.Models;

namespace ABCBankApps.Controllers
{
    public class CustomerApiController : ApiController
    {
        private ABCBank db = new ABCBank();

        // GET api/CustomerApi
        public IEnumerable<CustomersAcct> GetCustomersAccts()
        {
            var customersaccts = db.CustomersAccts.Include(c => c.StaffDetail);
            return customersaccts.AsEnumerable();
        }

        // GET api/CustomerApi/5
        public CustomersAcct GetCustomersAcct(int id)
        {
            CustomersAcct customersacct = db.CustomersAccts.Find(id);
            if (customersacct == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return customersacct;
        }

        // PUT api/CustomerApi/5
        public HttpResponseMessage PutCustomersAcct(int id, CustomersAcct customersacct)
        {
            if (ModelState.IsValid && id == customersacct.iAcNo)
            {
                db.Entry(customersacct).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/CustomerApi
        public HttpResponseMessage PostCustomersAcct(CustomersAcct customersacct)
        {
            if (ModelState.IsValid)
            {
                db.CustomersAccts.Add(customersacct);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customersacct);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customersacct.iAcNo }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CustomerApi/5
        public HttpResponseMessage DeleteCustomersAcct(int id)
        {
            CustomersAcct customersacct = db.CustomersAccts.Find(id);
            if (customersacct == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CustomersAccts.Remove(customersacct);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, customersacct);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}