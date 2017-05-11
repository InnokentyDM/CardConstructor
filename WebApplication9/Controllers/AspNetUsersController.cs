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
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "user, admin, manager")]
	[RequireHttps]
	public class AspNetUsersController : Controller
    {
        private WebApplication9.Models.Entities1 db = new Entities1();
		public WebApplication9.Models.AspNetUsers m_AspNetUsers;

        [Authorize(Roles = "admin, manager")]
        // GET: AspNetUsers
        public async Task<ActionResult> Index()
        {
           
            return View(await db.AspNetUsers.ToListAsync());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            //ViewBag.USER_ROLES = from c in db.AspNetUserRoles where 
            ViewBag.USER_CLAIMS = (List<AspNetUserClaims>) db.AspNetUserClaims.Where(c => c.User_Id == aspNetUsers.Id).ToList();
            var account = new AccountController();
            ViewBag.USER_ROLES = account.UserManager.GetRoles(aspNetUsers.Id);
            //ViewBag.USER_ROLES = db.AspNetUsers.Where(c => c.Id == aspNetUsers.Id).Select(c => c.AspNetRoles);
            //var roles = db.AspNetUsers.Where(c => c.Id == aspNetUsers.Id).Select(c => c.AspNetRoles).ToList();
            //List<AspNetRoles> rolesList = new List<AspNetRoles>();
            //foreach (var i in roles)
            //{
            //    rolesList.Add(i);
            //}

          
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserName,PasswordHash,SecurityStamp,Discriminator,Email,ConfirmedEmail")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUsers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNetUsers);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ROLES = new SelectList(db.AspNetRoles.GroupBy(c => c.Name).Select(g => g.FirstOrDefault()),"Name", "Name");
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,PasswordHash,SecurityStamp,Discriminator,Email,ConfirmedEmail")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {           
                db.Entry(aspNetUsers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Where(c => c.UserName == UserName).SingleOrDefault();
            var account = new AccountController();
            account.UserManager.AddToRoleAsync(aspNetUsers.Id, RoleName);
            ViewBag.ResultMessage = "Role created successfully !";
            // prepopulat roles for the view dropdown       
            return RedirectToAction("Details", new { id = aspNetUsers.Id });
        }

        // GET: AspNetUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUsers);
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
