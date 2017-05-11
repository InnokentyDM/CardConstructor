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
    public class CARD_CLAIMSController : Controller
    {
        private Entities1 db = new Entities1();
        [Authorize(Roles = "admin, manager")]
        // GET: CARD_CLAIMS
        public async Task<ActionResult> Index()
        {
            var cARD_CLAIMS = db.CARD_CLAIMS.Include(c => c.cards);
            return View(await cARD_CLAIMS.ToListAsync());
        }

        public async Task<ActionResult> groupedClaims()
        {
            var CARD_CLAIMS = db.CARD_CLAIMS.GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
            return View(await CARD_CLAIMS.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> groupedClaims([Bind(Include = "Id,ClaimType,ClaimValue,Card_id, published")] CARD_CLAIMS cARD_CLAIMS)
        {           
                var claims = db.CARD_CLAIMS.Where(c => c.ClaimValue == cARD_CLAIMS.ClaimValue);
                foreach (var i in claims)
                {
                    CARD_CLAIMS cc = db.CARD_CLAIMS.Find(i.Id);
                    CARD_CLAIMS firstCc = db.CARD_CLAIMS.Find(cARD_CLAIMS.Id);
                    cc.published = i.published;
                    db.Entry(cc).State = EntityState.Modified;
                    db.Entry(firstCc).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            var CARD_CLAIMS = db.CARD_CLAIMS.GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());

            return View(await CARD_CLAIMS.ToListAsync());
        }


        public async Task<ActionResult> getItemTypes()
        {
            var cARD_CLAIMS = db.CARD_CLAIMS.Include(c => c.cards).Where(c => c.ClaimType == "TYPE");
            return View(await cARD_CLAIMS.ToListAsync());
        }
        public async Task<ActionResult> getItemClaims()
        {
            var cARD_CLAIMS = db.CARD_CLAIMS.Include(c => c.cards).Where(c => c.ClaimType == "TYPE");
            return View(await cARD_CLAIMS.ToListAsync());
        }




        // GET: CARD_CLAIMS/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD_CLAIMS cARD_CLAIMS = await db.CARD_CLAIMS.FindAsync(id);
            if (cARD_CLAIMS == null)
            {
                return HttpNotFound();
            }
            return View(cARD_CLAIMS);
        }

        // GET: CARD_CLAIMS/Create
        public ActionResult Create()
        {
            ViewBag.Card_id = new SelectList(db.cards, "Id", "text");
            return View();
        }

        // POST: CARD_CLAIMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClaimType,ClaimValue,Card_id, published")] CARD_CLAIMS cARD_CLAIMS)
        {
            if (ModelState.IsValid)
            {
                db.CARD_CLAIMS.Add(cARD_CLAIMS);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Card_id = new SelectList(db.cards, "Id", "text", cARD_CLAIMS.Card_id);
            return View(cARD_CLAIMS);
        }

        // GET: CARD_CLAIMS/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD_CLAIMS cARD_CLAIMS = await db.CARD_CLAIMS.FindAsync(id);
            if (cARD_CLAIMS == null)
            {
                return HttpNotFound();
            }
            ViewBag.Card_id = new SelectList(db.cards, "Id", "text", cARD_CLAIMS.Card_id);
            return View(cARD_CLAIMS);
        }

        // POST: CARD_CLAIMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClaimType,ClaimValue,Card_id, published")] CARD_CLAIMS cARD_CLAIMS)
        {
            if (ModelState.IsValid)
            {
                var claims = db.CARD_CLAIMS.Where(c => c.ClaimValue == cARD_CLAIMS.ClaimValue).ToList();
                foreach (var i in claims)
                {
                    CARD_CLAIMS cc = db.CARD_CLAIMS.Find(i.Id);
                   
                    cc.published = cARD_CLAIMS.published;
                    db.Entry(cc).State = EntityState.Modified;           
                    db.SaveChanges();
                }
                //db.Entry(cARD_CLAIMS).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                    return RedirectToAction("Index");
            }
            ViewBag.Card_id = new SelectList(db.cards, "Id", "text", cARD_CLAIMS.Card_id);
            return View(cARD_CLAIMS);
        }

        // GET: CARD_CLAIMS/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CARD_CLAIMS cARD_CLAIMS = await db.CARD_CLAIMS.FindAsync(id);
            if (cARD_CLAIMS == null)
            {
                return HttpNotFound();
            }
            return View(cARD_CLAIMS);
        }

        // POST: CARD_CLAIMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CARD_CLAIMS cARD_CLAIMS = await db.CARD_CLAIMS.FindAsync(id);
            db.CARD_CLAIMS.Remove(cARD_CLAIMS);
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
