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
    [Authorize(Roles = "user, admin, manager")]
    [RequireHttps]
    public class elementsController : Controller
    {
        private Entities1 db = new Entities1();

        [Authorize(Roles = "admin, manager")]
        // GET: elements
        public async Task<ActionResult> Index()
        {
            var elements = db.elements.Include(e => e.ELEMENTS_TYPES).OrderByDescending(c => c.Id);
            return View(await elements.ToListAsync());
        }

        [Authorize(Roles = "admin, manager")]
        // GET: elements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            elements elements = await db.elements.FindAsync(id);
            if (elements == null)
            {
                return HttpNotFound();
            }
            return View(elements);
        }

        [Authorize(Roles = "admin, manager")]
        // GET: elements/Create
        public ActionResult Create()
        {
            ViewBag.type_name_id = new SelectList(db.ELEMENTS_TYPES, "Id", "TYPE_NAME");
            ViewBag.item_type_id = new SelectList(db.ITEM_TYPE, "Id", "NAME", "ITEM_TYPE");
            ViewBag.item_layout_id = new SelectList(db.layout, "Id", "name", "ITEM_LAYOUT");
            return View(new ImageViewModel());
        }

        // POST: elements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ImageUpload,type_name_id, item_type_id, item_layout_id")] ImageViewModel model)
        {
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
                var elements = new elements
                {
                    type_name_id = model.type_name_id
                };
                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    var uploadDir = "~/Files";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
                    var imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
                    model.ImageUpload.SaveAs(imagePath);
                    elements.path = imageUrl;
                    elements.item_type_id = model.item_type_id;
                    elements.type_name_id = model.type_name_id;
                    elements.item_layout_id = model.item_layout_id;
                    
                }
                db.elements.Add(elements);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.type_name_id = new SelectList(db.ELEMENTS_TYPES, "Id", "TYPE_NAME", model.type_name_id);
      
            return View(model);
        }

        // GET: elements/Edit/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            elements elements = await db.elements.FindAsync(id);
            if (elements == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_name_id = new SelectList(db.ELEMENTS_TYPES, "Id", "TYPE_NAME", elements.type_name_id);
            ViewBag.item_type_id = new SelectList(db.ITEM_TYPE, "Id", "NAME", elements.item_type_id);
            ViewBag.item_layout_id = new SelectList(db.layout, "Id", "name", elements.item_layout_id);
            return View(elements);
        }

        // POST: elements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,path,type_name_id,item_type_id,item_layout_id")] elements elements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elements).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.type_name_id = new SelectList(db.ELEMENTS_TYPES, "Id", "TYPE_NAME", elements.type_name_id);
            ViewBag.item_type_id = new SelectList(db.ITEM_TYPE, "Id", "NAME", elements.item_type_id);
            ViewBag.item_layout_id = new SelectList(db.layout, "Id", "name", elements.item_layout_id);
            return View(elements);
        }

        // GET: elements/Delete/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            elements elements = await db.elements.FindAsync(id);
            if (elements == null)
            {
                return HttpNotFound();
            }
            return View(elements);
        }

        // POST: elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            elements elements = await db.elements.FindAsync(id);
            db.elements.Remove(elements);
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
