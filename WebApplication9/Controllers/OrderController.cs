using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    [Authorize(Roles = "user, admin, manager")]
    [RequireHttps]
    public class OrderController : Controller
    {
        private Entities1 db = new Entities1();
        // GET: Order
        public ActionResult Index()
        {
            application apps = db.application.FirstOrDefault();
            if (apps != null)
            {
                OrderModel orderModel = new OrderModel { app = apps, Sum = apps.summ };
                return View(orderModel);
            }
            return HttpNotFound();
        }
    }
}