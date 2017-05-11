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
using WebApplication9.DAO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "user, admin, manager")]
	[RequireHttps]
	public class cardsController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
        private WebApplication9.DAO.ApplicationDAO applicationDAO = new ApplicationDAO();

        //Get image
        public ActionResult UploadImage(String imageData)
        {
            //byte[] data = Convert.FromBase64String(imageData);
            Session["imageSession"] = imageData;
            return RedirectToAction("Apply");
        }

        public ActionResult ManagerCards()
        {
            return View(db.cards.Where(c => c.published == true));
        }

        public async Task<ActionResult> GroupedCards()
        {
            //var cards = db.cards.Include(c => c.CARD_GROUP).Include(c => c.ITEM_TYPE1).Include(c => c.layout).GroupBy(c => c.CARD_GROUP);
            var cards = db.cards.Include(c => c.CARD_CLAIMS).Include(c => c.layout);
            return View(await cards.ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        // GET: cards
        public async Task<ActionResult> Index()
        {
            //var cards = db.cards.Include(c => c.AspNetUsers).Include(c => c.CARD_CLAIMS).Include(c => c.layout).Include(c => c.ITEM_TYPE).Select(c => new {Id = c.Id, layout_id = c.layout_id, text = c.text, preview = c.preview, published = c.published, item_type_id = c.ITEM_TYPE_ID, preview_2 = c.preview_2 }).OrderByDescending(c => c.Id);
            //List<cards> cardList = new List<Models.cards>();
            //foreach (var i in cards)
            //{
            //    cards card = new cards();
            //    card.Id = i.Id;
            //    card.layout_id = i.layout_id;
            //    card.text = i.text;
            //    card.preview = i.preview;
            //    card.published = i.published;
            //    card.ITEM_TYPE_ID = i.item_type_id;
            //    card.preview_2 = i.preview_2;
            //    card.layout = db.layout.Find(i.layout_id);
            //    card.ITEM_TYPE = db.ITEM_TYPE.Find(i.item_type_id);
            //    cardList.Add(card);
            //}

            List<cards> cardList = db.cards.ToList();
            return View(cardList);
        }
            
        // GET: cards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cards cards = await db.cards.FindAsync(id);
            if (cards == null)
            {
                return HttpNotFound();
            }
            return View(cards);
        }

        // GET: cards/Create
        public ActionResult Create()
        {

            ViewBag.COLOR_CLAIM = new SelectList(applicationDAO.getColorClaims(), "Id", "ClaimValue");
            ViewBag.STYLE_CLAIM = new SelectList(applicationDAO.getStyleClaims(), "Id", "ClaimValue");
            ViewBag.GROUP_CLAIM = new SelectList(applicationDAO.getGroupClaims(), "Id", "ClaimValue");

            Dictionary<int, string> layoutO = new Dictionary<int, string>();
            var layouts = applicationDAO.getAllLayout();
            foreach (var item in layouts)
            {
                string param = item.height + " " + item.width;
                layoutO.Add(item.Id, param);
            }

            ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME");
            return View();
        }

        // POST: cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,layout_id,text,image,USER_ID,published,ITEM_TYPE_ID, CARD_CLAIMS, preview, preview_2")] cards cards)
        {
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;              
                cards.USER_ID = user.Id;

                
                db.cards.Add(cards);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.USER_ID = new SelectList(db.AspNetUsers, "Id", "UserName", cards.USER_ID);
            ViewBag.layout_id = new SelectList(db.layout, "Id", "width", cards.layout_id);
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cards.ITEM_TYPE_ID);
            return View(cards);
        }

        // GET: cards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cards cards = await db.cards.FindAsync(id);
            if (cards == null)
            {
                return HttpNotFound();
            }

            ViewBag.COLOR_CLAIM = new SelectList(applicationDAO.getColorClaims(), "Id", "ClaimValue");
            ViewBag.STYLE_CLAIM = new SelectList(applicationDAO.getStyleClaims(), "Id", "ClaimValue");
            ViewBag.GROUP_CLAIM = new SelectList(applicationDAO.getGroupClaims(), "Id", "ClaimValue");

            Dictionary<int, string> layoutO = new Dictionary<int, string>();
            var layouts = applicationDAO.getAllLayout();
            foreach (var item in layouts)
            {
                string param = item.height + " " + item.width;
                layoutO.Add(item.Id, param);
            }

            ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
            ViewBag.USER_ID = new SelectList(db.AspNetUsers, "Id", "UserName", cards.USER_ID);
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cards.ITEM_TYPE_ID);
            return View(cards);
        }

        // POST: cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,layout_id,text,image,USER_ID,published,ITEM_TYPE_ID")] cards cards)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cards).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.USER_ID = new SelectList(db.AspNetUsers, "Id", "UserName", cards.USER_ID);
            ViewBag.layout_id = new SelectList(db.layout, "Id", "width", cards.layout_id);
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cards.ITEM_TYPE_ID);
            return View(cards);
        }

        // GET: cards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cards cards = await db.cards.FindAsync(id);
            if (cards == null)
            {
                return HttpNotFound();
            }
            return View(cards);
        }

        // POST: cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            cards cards = await db.cards.FindAsync(id);
            List<CARD_CLAIMS> claimsList = new List<CARD_CLAIMS>();
            var claims = db.CARD_CLAIMS.Where(c => c.Card_id == cards.Id);
            claimsList = claims.ToList();
            foreach (var i in claimsList)
            {
                db.CARD_CLAIMS.Remove(i);
                await db.SaveChangesAsync();
            }
            db.cards.Remove(cards);
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
        //Add claim method 
        //Use for add new claim fields on create and edit card view
        //Method use claims_partial/CLAIM_PARTIAL partial view
        public async Task<ActionResult> addClaimFields()
        {
            return PartialView("CLAIM_PARTIAL");
        }


        public ActionResult UserCardsList()
        {
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            string userId = user.Id;
            List<cards> cards = db.cards.Where(c => c.USER_ID == userId).ToList();
            return View(cards);
        }

        [Authorize(Roles = "user, admin, manager")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult SaveTo(cards model)
        {
            if (ModelState.IsValid)
            {
                cards card = new cards();
                card.IMAGES.Image = model.IMAGES.Image;
                card.IMAGES.Image_2 = model.IMAGES.Image_2;
                card.ITEM_TYPE_ID = model.ITEM_TYPE_ID;
                card.layout_id = model.layout_id;
                card.published = false;
                card.text = model.text;
                card.preview = model.preview;
                card.preview_2 = model.preview_2;
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                string userId = user.Id;
                //model.layout = _entities.layout.Find(model.layout_id);
                card.USER_ID = userId;
                //card.published = false;
                //applicationDAO.AddNewCard(card);
                db.cards.Add(card);
                db.SaveChanges();
            }
            return RedirectToAction("UserCardsList");
        }
    }


   
}
