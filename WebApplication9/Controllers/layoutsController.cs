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
using System.IO;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "admin, manager")]
	[RequireHttps]
	public class layoutsController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
		public WebApplication9.Models.layout m_layout;

        [Authorize(Roles = "admin, manager")]
        // GET: layouts
        public async Task<ActionResult> Index()
        {
            var layout = db.layout.Include(l => l.ITEM_TYPE);
            return View(await layout.ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        // GET: layouts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            layout layout = await db.layout.FindAsync(id);
            if (layout == null)
            {
                return HttpNotFound();
            }
            return View(layout);
        }
        [Authorize(Roles = "admin, manager")]
        // GET: layouts/Create
        public ActionResult Create()
        {
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME");
            return View(new LayoutViewModel());
        }

        // POST: layouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ImageUpload,width,height,name,item_type_id")] LayoutViewModel model)
        {
            var context = HttpContext;
            string file = HttpContext.Request.Form["ImageUpload"];
            HttpPostedFileBase f = HttpContext.Request.Files[file];
            var validImageTypes = new string[]
               {
                     "image/jpeg",
                     "image/png"
               };
            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }
            if (ModelState.IsValid)
            {
                
                var uploadDir = "~/Files";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                model.ImageUpload.SaveAs(imagePath);
                model.layout.Image = imageUrl;
                db.layout.Add(model.layout);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", model.layout.ITEM_TYPE_ID);
            return View(model);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: layouts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutViewModel lvm = new LayoutViewModel();
            lvm.layout = await db.layout.FindAsync(id);
            if (lvm.layout == null)
            {
                return HttpNotFound();
            }
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", lvm.layout.ITEM_TYPE_ID);
            return View(lvm);
        }

        // POST: layouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "layout, ImageUpload")] LayoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var uploadDir = "~/Files";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);            
                model.ImageUpload.SaveAs(imagePath);
                Uri fromPath = new Uri(imagePath);
                Uri toPath = new Uri(Server.MapPath("./Files"));
                Uri path = toPath.MakeRelativeUri(fromPath);
                model.layout.Image = path.ToString();
                db.Entry(model.layout).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ITEM_TYPE_ID = new SelectList(db.ITEM_TYPE, "Id", "NAME", model.layout.ITEM_TYPE_ID);
            return View(model.layout);
        }

        // GET: layouts/Delete/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            layout layout = await db.layout.FindAsync(id);
            if (layout == null)
            {
                return HttpNotFound();
            }
            return View(layout);
        }

        [Authorize(Roles = "admin, manager")]
        // POST: layouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            layout layout = await db.layout.FindAsync(id);
            db.layout.Remove(layout);
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
