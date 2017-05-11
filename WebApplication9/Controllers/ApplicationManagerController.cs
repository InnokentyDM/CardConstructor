using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication9.DAO;
using WebApplication9.Models;

namespace WebApplication9.Controllers
{
    public class ApplicationManagerController : Controller
    {
        // GET: ApplicationManager
        public ActionResult Index()
        {
            return View();
        }

        public void UploadSvgImage()
        {

        }
    }
}