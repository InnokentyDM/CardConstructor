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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "user, admin, manager")]
    [RequireHttps]
    public class favoritesController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: favorites
        public async Task<ActionResult> Index()
        {
            var favorite = db.favorite.Include(f => f.AspNetUsers).Include(f => f.cards);
            return View(await favorite.ToListAsync());
        }

        public ActionResult MyFavorite()
        {
            return View("MyFavorite");
        }

        public ActionResult FavoriteCardsList()
        {
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            var favorite = db.favorite.Where(c => c.user_id == user.Id);
            var cards = db.cards;
            List<cards> favoriteCards = new List<Models.cards>();
            foreach (var i in cards)
            {
                foreach (var item in favorite)
                {
                    if (i.Id == item.card_id)
                    {
                        favoriteCards.Add(i);
                    }
                }
            }
            return PartialView("Cards", favoriteCards);
        }

        // GET: favorites/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = await db.favorite.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        // GET: favorites/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.card_id = new SelectList(db.cards, "Id", "text");
            return View();
        }

        // POST: favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,user_id,card_id")] favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.favorite.Add(favorite);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", favorite.user_id);
            ViewBag.card_id = new SelectList(db.cards, "Id", "text", favorite.card_id);
            return View(favorite);
        }

        [HttpPost]
        [Authorize(Roles = "user, admin, manager")]
        public string addToFavorite(int id)
        {
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            favorite fav = new favorite();
            fav.user_id = user.Id;
            fav.card_id = id;
            var favs = db.favorite.Where(c => c.user_id == user.Id);
            bool isThere = false;
            favorite delfav = new favorite();
            if (favs.Any())
            {
                foreach (var i in favs)
                {
                    if (i.card_id == id)
                    {
                        delfav = db.favorite.Find(i.Id);
                        isThere = true;              
                    }              
                }
            }
            if (isThere)
            {
                db.favorite.Remove(delfav);
                db.SaveChanges();
                return "removed";
            }
            else
            {
                db.favorite.Add(fav);
                db.SaveChanges();
                return "added";
            }
        }

        // GET: favorites/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = await db.favorite.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", favorite.user_id);
            ViewBag.card_id = new SelectList(db.cards, "Id", "text", favorite.card_id);
            return View(favorite);
        }

        // POST: favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,user_id,card_id")] favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorite).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", favorite.user_id);
            ViewBag.card_id = new SelectList(db.cards, "Id", "text", favorite.card_id);
            return View(favorite);
        }

        // GET: favorites/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = await db.favorite.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        // POST: favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            favorite favorite = await db.favorite.FindAsync(id);
            db.favorite.Remove(favorite);
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
