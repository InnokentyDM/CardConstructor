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
	public class ITEM_TYPEController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
		public WebApplication9.Models.ITEM_TYPE m_ITEM_TYPE;

        [Authorize(Roles = "admin, manager")]
        // GET: ITEM_TYPE
        public async Task<ActionResult> Index()
        {
            return View(await db.ITEM_TYPE.ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        // GET: ITEM_TYPE/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM_TYPE iTEM_TYPE = await db.ITEM_TYPE.FindAsync(id);
            if (iTEM_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(iTEM_TYPE);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: ITEM_TYPE/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        // POST: ITEM_TYPE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,NAME,DESCRIPTION")] ITEM_TYPE iTEM_TYPE)
        {
            if (ModelState.IsValid)
            {
                db.ITEM_TYPE.Add(iTEM_TYPE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(iTEM_TYPE);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: ITEM_TYPE/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM_TYPE iTEM_TYPE = await db.ITEM_TYPE.FindAsync(id);
            if (iTEM_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(iTEM_TYPE);
        }

        // POST: ITEM_TYPE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NAME,DESCRIPTION")] ITEM_TYPE iTEM_TYPE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iTEM_TYPE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(iTEM_TYPE);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: ITEM_TYPE/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ITEM_TYPE iTEM_TYPE = await db.ITEM_TYPE.FindAsync(id);
            if (iTEM_TYPE == null)
            {
                return HttpNotFound();
            }
            return View(iTEM_TYPE);
        }

        // POST: ITEM_TYPE/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ITEM_TYPE iTEM_TYPE = await db.ITEM_TYPE.FindAsync(id);
            db.ITEM_TYPE.Remove(iTEM_TYPE);
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
