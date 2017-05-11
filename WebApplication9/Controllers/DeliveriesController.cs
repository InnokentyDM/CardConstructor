using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "user, admin, manager")]
    [RequireHttps]
    public class DeliveriesController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Deliveries
        public async Task<ActionResult> Index()
        {
            var delivery = db.Delivery.Include(d => d.Towns);
            return View(await delivery.ToListAsync());
        }

        // GET: Deliveries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = await db.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create()
        {
            ViewBag.Town_id = new SelectList(db.Towns, "Id", "Name");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Town_id,Street,House,Apartment")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Delivery.Add(delivery);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Town_id = new SelectList(db.Towns, "Id", "Name", delivery.Town_id);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = await db.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.Town_id = new SelectList(db.Towns, "Id", "Name", delivery.Town_id);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Town_id,Street,House,Apartment")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Town_id = new SelectList(db.Towns, "Id", "Name", delivery.Town_id);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = await db.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Delivery delivery = await db.Delivery.FindAsync(id);
            db.Delivery.Remove(delivery);
            await db.SaveChangesAsync();
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
