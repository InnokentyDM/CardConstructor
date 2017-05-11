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
    public class TownsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Towns
        public async Task<ActionResult> Index()
        {
            return View(await db.Towns.ToListAsync());
        }

        // GET: Towns/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Towns towns = await db.Towns.FindAsync(id);
            if (towns == null)
            {
                return HttpNotFound();
            }
            return View(towns);
        }

        // GET: Towns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Towns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Towns towns)
        {
            if (ModelState.IsValid)
            {
                db.Towns.Add(towns);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(towns);
        }

        // GET: Towns/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Towns towns = await db.Towns.FindAsync(id);
            if (towns == null)
            {
                return HttpNotFound();
            }
            return View(towns);
        }

        // POST: Towns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Towns towns)
        {
            if (ModelState.IsValid)
            {
                db.Entry(towns).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(towns);
        }

        // GET: Towns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Towns towns = await db.Towns.FindAsync(id);
            if (towns == null)
            {
                return HttpNotFound();
            }
            return View(towns);
        }

        // POST: Towns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Towns towns = await db.Towns.FindAsync(id);
            db.Towns.Remove(towns);
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
