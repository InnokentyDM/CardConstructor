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
    [Authorize(Roles = "admin, manager")]
	[RequireHttps]
	public class COUNTsController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
		public WebApplication9.Models.COUNT m_COUNT;


        [Authorize(Roles = "admin, manager")]
        // GET: COUNTs
        public async Task<ActionResult> Index()
        {
            var cOUNT = db.COUNT.Include(c => c.ITEM_TYPE);
            return View(await cOUNT.ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        // GET: COUNTs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COUNT cOUNT = await db.COUNT.FindAsync(id);
            if (cOUNT == null)
            {
                return HttpNotFound();
            }
            return View(cOUNT);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: COUNTs/Create
        public ActionResult Create()
        {
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME");
            return View();
        }

        // POST: COUNTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ITEM_TYPE_ID,COUNT_VALUE,PRICE")] COUNT cOUNT)
        {
            if (ModelState.IsValid)
            {
                db.COUNT.Add(cOUNT);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cOUNT.ITEM_TYPE_ID);
            return View(cOUNT);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: COUNTs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COUNT cOUNT = await db.COUNT.FindAsync(id);
            if (cOUNT == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cOUNT.ITEM_TYPE_ID);
            return View(cOUNT);
        }

        // POST: COUNTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ITEM_TYPE_ID,COUNT_VALUE,PRICE")] COUNT cOUNT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cOUNT).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cOUNT.ITEM_TYPE_ID);
            return View(cOUNT);
        }


        [Authorize(Roles = "admin, manager")]
        // GET: COUNTs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COUNT cOUNT = await db.COUNT.FindAsync(id);
            if (cOUNT == null)
            {
                return HttpNotFound();
            }
            return View(cOUNT);
        }

        // POST: COUNTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            COUNT cOUNT = await db.COUNT.FindAsync(id);
            db.COUNT.Remove(cOUNT);
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
