using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication9.DAO;
using WebApplication9.Models;
using WebApplication9.Models.addCards;

namespace WebApplication9.Controllers
{

    [RequireHttps]
	public class addCardsController : Controller
    {
        private WebApplication9.DAO.ApplicationDAO applicationDAO = new ApplicationDAO();
        private WebApplication9.Models.Entities1 db = new Entities1();
		public WebApplication9.Models.cards m_cards;
        // GET: addCards
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "admin, manager")]
        //Choose item type when trying to create card layout
        public ActionResult chooseItemType()
        {
            return View(applicationDAO.getAllItemTypes());
        }

        [Authorize(Roles = "user, admin, manager")]
        // GET: cards/Create
        public ActionResult Create(String type_id)
         {
            var types = db.ITEM_TYPE.Where(c => c.NAME == type_id).ToList();
            ITEM_TYPE item_type = new ITEM_TYPE();
            foreach (var i in types)
            {
                item_type = i;
            }
            var addCards = new addCardModel();
            var exSearch = new Models.addCards.ExtandedSearchModel();
            var availItems = new List<checkBoxListModel>();
            var selectedItems = new List<checkBoxListModel>();
            cards card = new cards();
            card.ITEM_TYPE_ID = item_type.Id;
            addCards.cards = card;
            addCards.ITEM_TYPE_ID = item_type.Id;
            var claims = applicationDAO.getAllCardClaims();
            int count = 0;
            foreach (var item in claims)
            {
                if (item.ClaimType != "TYPE")
                {
                    availItems.Add(new checkBoxListModel(item.ClaimValue, item.ClaimValue));
                }
                count++;
            }
            addCards.availItems = availItems;

            //ViewBag.COLOR_CLAIM = new SelectList(applicationDAO.getColorClaims(), "Id", "ClaimValue");
            //ViewBag.STYLE_CLAIM = new SelectList(applicationDAO.getStyleClaims(), "Id", "ClaimValue");
            //ViewBag.GROUP_CLAIM = new SelectList(applicationDAO.getGroupClaims(), "Id", "ClaimValue");

            Dictionary<int, string> layoutO = new Dictionary<int, string>();
            var layouts = db.layout.Where(c => c.ITEM_TYPE_ID == item_type.Id);
            foreach (var item in layouts)
            {
                string param = item.width + " " + item.height;
                layoutO.Add(item.Id, param);
            }
            ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
            //ViewBag.layout_id = new SelectList(db.layout.Where(c => c.ITEM_TYPE_ID == item_type.Id).ToList(), "Id", "name");
            return View(addCards);
        }

        public void UploadImage(String imageData)
        {
            //byte[] data = Convert.FromBase64String(imageData);
            Session["imageSession"] = imageData;
            
        }


        public Uri imageFromBase64(string imageData)
        {
            try { 
            var base64 = Regex.Match(imageData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

            byte[] imgData = Convert.FromBase64String(base64);
            string fileName = string.Format(@"{0}.png", Guid.NewGuid());
            string fileNameWithPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            using (FileStream fs = new FileStream(fileNameWithPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = imgData;
                    bw.Write(data);
                    bw.Close();
                }
            }
            Uri fromPath = new Uri(fileNameWithPath);
            Uri toPath = new Uri(Server.MapPath("./Files"));
            Uri path = toPath.MakeRelativeUri(fromPath);
            return path;
            }
            catch (Exception except)
            {
                Debug.Assert(except != null, "It's definitely null");
                Console.WriteLine(except.ToString());
                return null;
            }
        }

        private string writeFile(string text)
        {
            if (text != null) { 
            string fileName = string.Format(@"{0}.txt", Guid.NewGuid());
            string fileNameWithPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            using (StreamWriter sw = new StreamWriter(fileNameWithPath))
            {
                sw.WriteLine(text);
            }       
            return fileNameWithPath;
            }
            return null;
        }

        // POST: cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "uesr, admin, manager")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(addCardModel addCardModel)
        {
            try {
                if (ModelState.IsValid)
                {
                    cards card = new cards();
                    CARD_CLAIMS card_claims = new CARD_CLAIMS();
                    var selectedItems = new List<checkBoxListModel>();
                    var postedItemsIds = new string[0];
                    var availItems = new List<checkBoxListModel>();
                    if (addCardModel.postedItems == null) addCardModel.postedItems = new PostedItems();

                    if (addCardModel.postedItems.IDs != null && addCardModel.postedItems.IDs.Any())
                    {
                        postedItemsIds = addCardModel.postedItems.IDs;
                    }

                    string userName = User.Identity.Name;
                    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    var userManager = new UserManager<ApplicationUser>(store);
                    ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                    card.text = addCardModel.cards.text;
                    card.published = addCardModel.cards.published;
                    card.layout_id = addCardModel.layout_id;
                    card.IMAGES = new IMAGES();
                    card.IMAGES.Image = writeFile(addCardModel.cards.IMAGES.Image);
                    card.IMAGES.Image_2 = writeFile(addCardModel.cards.IMAGES.Image_2);
                    card.preview = imageFromBase64(addCardModel.cards.preview).ToString();
                    if (addCardModel.cards.preview_2 != null)
                    { 
                    card.preview_2 = imageFromBase64(addCardModel.cards.preview_2).ToString();
                    }
                    int? ITEM_TYPE_ID = 0;
                    var type_ids = db.layout.Where(c => c.Id == addCardModel.layout_id);
                    foreach (var i in type_ids)
                    {
                        ITEM_TYPE_ID = i.ITEM_TYPE_ID;
                    }
                    card.ITEM_TYPE_ID = ITEM_TYPE_ID;
                    card.USER_ID = user.Id;
                    db.cards.Add(card);
                    await db.SaveChangesAsync();
                    if (postedItemsIds.Any())
                    {
                        var claimsQuery = applicationDAO.selectedItemsForExtandedSearch(addCardModel.postedItems.IDs).ToList();
                        foreach (var i in claimsQuery)
                        {
                            card_claims.ClaimType = i.ClaimType;
                            card_claims.ClaimValue = i.ClaimValue;
                            card_claims.Card_id = card.Id;
                            db.CARD_CLAIMS.Add(card_claims);
                            await db.SaveChangesAsync();
                        }
                    }


                    return RedirectToAction("../cards/Index");
                }
                else {
                    var exSearch = new Models.addCards.ExtandedSearchModel();
                    var availItems = new List<checkBoxListModel>();
                    var selectedItems = new List<checkBoxListModel>();
                    var types = db.ITEM_TYPE.Where(c => c.Id == addCardModel.ITEM_TYPE_ID).ToList();
                    ITEM_TYPE item_type = new ITEM_TYPE();
                    foreach (var i in types)
                    {
                        item_type = i;
                    }
                    Dictionary<int, string> layoutO = new Dictionary<int, string>();
                    var layouts = db.layout.Where(c => c.ITEM_TYPE_ID == item_type.Id);
                    foreach (var item in layouts)
                    {
                        string param = item.width + " " + item.height;
                        layoutO.Add(item.Id, param);
                    }
                    ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
                    var claims = applicationDAO.getAllCardClaims();
                    int count = 0;
                    foreach (var item in claims)
                    {
                        if (item.ClaimType != "TYPE")
                        {
                            availItems.Add(new checkBoxListModel(item.ClaimValue, item.ClaimValue));
                        }
                        count++;
                    }
                    addCardModel.availItems = availItems;
                    return View(addCardModel);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
                return View("Возникла ошибка");
            }

          
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

            var addCards = new addCardModel();
            var exSearch = new Models.addCards.ExtandedSearchModel();
            var availItems = new List<checkBoxListModel>();
            var selectedItems = new List<checkBoxListModel>();
            PostedItems postedItems = new PostedItems();
            

            List<CARD_CLAIMS> cardClaims = applicationDAO.getCardClaims(cards.Id).ToList();
            postedItems.IDs = new string[cardClaims.Count];
            foreach (var i in cardClaims)
            {
                selectedItems.Add(new checkBoxListModel(i.ClaimValue, i.ClaimValue));
            }
            addCards.selectedItems = selectedItems;
            addCards.cards = cards;

            //Select data for card claims check box list
            var claims = applicationDAO.getAllCardClaims();
            
            foreach (var item in claims)
            {
                if (item.ClaimType != "TYPE")
                {
                    availItems.Add(new checkBoxListModel(item.ClaimValue, item.ClaimValue));
                }            
            }
            addCards.availItems = availItems;

            //Set item_type and layouts for this item_type
            var types = db.ITEM_TYPE.Where(c => c.Id == cards.ITEM_TYPE_ID).ToList();
            ITEM_TYPE item_type = new ITEM_TYPE();
            foreach (var i in types)
            {
                item_type = i;
            }
            Dictionary<int, string> layoutO = new Dictionary<int, string>();
            var layouts = db.layout.Where(c => c.ITEM_TYPE_ID == item_type.Id);
            foreach (var item in layouts)
            {
                string param = item.height + " " + item.width;
                layoutO.Add(item.Id, param);
            }
            ViewBag.layout_id = new SelectList(layoutO, "Key", "Value", cards.layout_id);      
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", cards.ITEM_TYPE_ID);
            addCards.availItems = availItems;          
            return View(addCards);
        }

        // POST: cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(addCardModel addCards)
        {
            if (ModelState.IsValid)
            {
                var res = db.cards.SingleOrDefault(c => c.Id == addCards.cards.Id);
                res.IMAGES.Image = addCards.cards.IMAGES.Image;
                res.IMAGES.Image_2 = addCards.cards.IMAGES.Image_2;
                res.published = addCards.cards.published;
                res.text = addCards.cards.text;
                
                CARD_CLAIMS card_claims = new CARD_CLAIMS();
                var selectedItems = new List<checkBoxListModel>();
                var postedItemsIds = new string[0];
                var availItems = new List<checkBoxListModel>();
                if (addCards.postedItems == null) addCards.postedItems = new PostedItems();

                if (addCards.postedItems.IDs != null && addCards.postedItems.IDs.Any())
                {
                    postedItemsIds = addCards.postedItems.IDs;
                }
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                res.USER_ID = user.Id;

                int? ITEM_TYPE_ID = 0;
                
                if (addCards.layout_id == null)
                { 
                var type_ids = db.cards.Where(c => c.Id == addCards.cards.Id);
                    foreach (var i in type_ids)
                    {
                        ITEM_TYPE_ID = i.ITEM_TYPE_ID;
                    }
                }
                else
                {
                    var type_ids = db.layout.Where(c => c.Id == addCards.layout_id);
                    foreach (var i in type_ids)
                    {
                        ITEM_TYPE_ID = i.ITEM_TYPE_ID;
                    }
                }
                
                res.ITEM_TYPE_ID = ITEM_TYPE_ID;
                
                //res = addCards.cards;
                db.Entry(res).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (postedItemsIds.Any())
                {
                    List<CARD_CLAIMS> cardClaims = applicationDAO.getCardClaims(addCards.cards.Id).ToList(); //CLaims of the added card
                    List<CARD_CLAIMS> claimsQuery = applicationDAO.selectedItemsForExtandedSearch(addCards.postedItems.IDs).ToList(); //All claims of cards
                    CARD_CLAIMS current_claim = new CARD_CLAIMS();
                    //var cardClaimsCo = db.CARD_CLAIMS;
                    //int c = 0;

                    foreach (var i in cardClaims)
                    {
                        CARD_CLAIMS cardclaim = await db.CARD_CLAIMS.FindAsync(i.Id);
                        db.CARD_CLAIMS.Remove(cardclaim);
                        await db.SaveChangesAsync();
                    }                   
                    foreach (CARD_CLAIMS i in claimsQuery)
                    {
                        current_claim.ClaimType = i.ClaimType;
                        current_claim.ClaimValue = i.ClaimValue;
                        current_claim.Card_id = res.Id;
                        db.CARD_CLAIMS.Add(current_claim);
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("../cards/Index");
            }
           
            return View("addCards/index");
        }

    }
}