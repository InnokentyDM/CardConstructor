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
	public class applicationsController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
        const int pageSize = 8;
		public WebApplication9.Models.application m_application;
        [Authorize(Roles = "admin, manager")]
        // GET: applications
        public ActionResult Index(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AppItems", getApplicationPage(page));
            }
            return View(getApplicationPage(page));
        }

        private List<application> getApplicationPage(int page=1)
        {
            var itemsToSkip = page * pageSize;
            List<application> application = db.application.Include(a => a.COUNT).Include(a => a.materials).Include(a => a.AspNetUsers).Include(a => a.cards).OrderBy(t => t.Id).Skip(itemsToSkip).Take(pageSize).OrderByDescending(c => c.Id).ToList();
            return application;
        }
        [Authorize(Roles = "admin, manager")]
        // GET: applications/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            application application = await db.application.FindAsync(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        [Authorize(Roles = "user, admin, manager")]
        // GET: applications/Create
        public ActionResult Create()
        {
            ViewBag.count_id = new SelectList(db.COUNT, "Id", "Id");
            ViewBag.material_id = new SelectList(db.materials, "Id", "description");
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.vc_id = new SelectList(db.cards, "Id", "text");
            return View();
        }

        // POST: applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "user, admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,vc_id,user_id,insertionDate,finishDate,status,count_id,summ,material_id")] application application)
        {
            if (ModelState.IsValid)
            {
                db.application.Add(application);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.count_id = new SelectList(db.COUNT, "Id", "Id", application.count_id);
            ViewBag.material_id = new SelectList(db.materials, "Id", "description", application.material_id);
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", application.user_id);
            ViewBag.vc_id = new SelectList(db.cards, "Id", "text", application.vc_id);
            return View(application);
        }

        // GET: applications/Edit/5
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            application application = await db.application.FindAsync(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.count_id = new SelectList(db.COUNT, "Id", "Id", application.count_id);
            ViewBag.material_id = new SelectList(db.materials, "Id", "description", application.material_id);
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", application.user_id);
            ViewBag.vc_id = new SelectList(db.cards, "Id", "text", application.vc_id);
            return View(application);
        }

        // POST: applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,vc_id,user_id,insertionDate,finishDate,status,count_id,summ,material_id")] application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.count_id = new SelectList(db.COUNT, "Id", "Id", application.count_id);
            ViewBag.material_id = new SelectList(db.materials, "Id", "description", application.material_id);
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "UserName", application.user_id);
            ViewBag.vc_id = new SelectList(db.cards, "Id", "text", application.vc_id);
            return View(application);
        }
        [Authorize(Roles = "admin, manager")]
        // GET: applications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            application application = await db.application.FindAsync(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            application application = await db.application.FindAsync(id);
            db.application.Remove(application);
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


        [Authorize(Roles = "user, admin, manager")]
        //InnokentyDM code starts here
        public ActionResult UserApplicationList(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ManagerApp", getUserApplicationList(page));
            }
            return View(getUserApplicationList(page));
        }
        [Authorize(Roles = "admin, manager")]
        public ActionResult ManagerApplicationList(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ManagerApp", getManagerApplicationList(page));
            }
            return View("ManagerApplicationList", getManagerApplicationList(page));
        }
        [Authorize(Roles = "admin, manager")]
        public ActionResult PrintApplicationList(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ManagerApp", getPrintApplicationList(page));
            }
            return View("ManagerApplicationList", getPrintApplicationList(page));
        }
        [Authorize(Roles = "admin, manager")]
        public ActionResult DoneApplicationList(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ManagerApp", getDoneApplicationList(page));
            }
            return View("ManagerApplicationList", getDoneApplicationList(page));
        }


        private List<application> getUserApplicationList(int page = 1)
        {
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            string userId = user.Id;
            List<application> apps = db.application.Where(c => c.user_id == userId).ToList();
            return apps;
        }

        private List<application> getManagerApplicationList(int page = 1)
        {
            var itemsToSkip = page * pageSize;
            List<application> apps = db.application.Where(c => c.status == "Обработка заявки").OrderByDescending(c => c.Id).Skip(itemsToSkip).Take(pageSize).ToList();
            return apps;
        }

        private List<application> getPrintApplicationList(int page = 1)
        {
            var itemsToSkip = page * pageSize;
            List<application> apps = db.application.Where(c => c.status == "Печать").OrderByDescending(c => c.Id).Skip(itemsToSkip).Take(pageSize).ToList();
            return apps;
        }

        private List<application> getDoneApplicationList(int page = 1)
        {
            var itemsToSkip = page * pageSize;
            List<application> apps = db.application.Where(c => c.status == "Готово").OrderByDescending(c => c.Id).Skip(itemsToSkip).Take(pageSize).ToList();
            return apps;
        }
        [Authorize(Roles = "admin, manager")]
        public ActionResult UpdateStatus(int id, string status)
        {
            application app = db.application.Find(id);
            db.Entry(app).State = EntityState.Modified;
            switch (status)
            {
                case ("ManagerApplicationList"):
                    app.status = "Печать";                
                    break;
                case ("PrintApplicationList"):
                    app.status = "Готово";
                    break;
            }
            db.SaveChanges();
            return RedirectToAction(status);
        }
        }

        
}

