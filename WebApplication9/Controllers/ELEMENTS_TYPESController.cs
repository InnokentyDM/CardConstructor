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
    [RequireHttps]
    public class ELEMENTS_TYPESController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: ELEMENTS_TYPES
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Index()
        {
            return View(await db.ELEMENTS_TYPES.ToListAsync());
        }

        // GET: ELEMENTS_TYPES/Details/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ELEMENTS_TYPES eLEMENTS_TYPES = await db.ELEMENTS_TYPES.FindAsync(id);
            if (eLEMENTS_TYPES == null)
            {
                return HttpNotFound();
            }
            return View(eLEMENTS_TYPES);
        }

        // GET: ELEMENTS_TYPES/Create
        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ELEMENTS_TYPES/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TYPE_NAME")] ELEMENTS_TYPES eLEMENTS_TYPES)
        {
            if (ModelState.IsValid)
            {
                db.ELEMENTS_TYPES.Add(eLEMENTS_TYPES);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(eLEMENTS_TYPES);
        }

        // GET: ELEMENTS_TYPES/Edit/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ELEMENTS_TYPES eLEMENTS_TYPES = await db.ELEMENTS_TYPES.FindAsync(id);
            if (eLEMENTS_TYPES == null)
            {
                return HttpNotFound();
            }
            return View(eLEMENTS_TYPES);
        }

        // POST: ELEMENTS_TYPES/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TYPE_NAME")] ELEMENTS_TYPES eLEMENTS_TYPES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eLEMENTS_TYPES).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eLEMENTS_TYPES);
        }

        // GET: ELEMENTS_TYPES/Delete/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ELEMENTS_TYPES eLEMENTS_TYPES = await db.ELEMENTS_TYPES.FindAsync(id);
            if (eLEMENTS_TYPES == null)
            {
                return HttpNotFound();
            }
            return View(eLEMENTS_TYPES);
        }

        // POST: ELEMENTS_TYPES/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ELEMENTS_TYPES eLEMENTS_TYPES = await db.ELEMENTS_TYPES.FindAsync(id);
            db.ELEMENTS_TYPES.Remove(eLEMENTS_TYPES);
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
