using WebApplication9.DAO;
using WebApplication9.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication9.Models.exSearch;
using System.Net.Mail;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Net;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace WebApplication9.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDAO applicationDAO = new ApplicationDAO();
        private Entities1 _entities = new Entities1();

        public ActionResult Menu()
        {
            //var itemTypes = _entities.ITEM_TYPE;
            return PartialView("Menu", _entities.ITEM_TYPE);
        }

        //Возвращает виды макетов (визитки, листовки)
        public ActionResult ItemTypes()
        {
            //var cards = new List<cards>(_entities.cards.OrderByDescending(c => c.Id).Where(c => c.published == true && c.ITEM_TYPE_ID == 1).GroupBy(c => c.layout_id).SelectMany(c => c.Take(10)).ToList());
            //var lists = new List<cards>(_entities.cards.OrderByDescending(c => c.Id).Where(c => c.published == true && c.ITEM_TYPE_ID == 2).GroupBy(c => c.layout_id).SelectMany(c => c.Take(10)).ToList());
            var cards = new List<KeyValuePair<layout, List<cards>>>();
            
            foreach (var item in _entities.ITEM_TYPE)
            {
                List<cards> tmpList = new List<Models.cards>();
                foreach (var i in _entities.layout)
                {
                    int width = Int32.Parse(i.width);
                    int height = Int32.Parse(i.height);
                    if (width > height)
                    {
                    tmpList = _entities.cards.OrderByDescending(c => c.Id).Where(c => c.published == true && c.ITEM_TYPE_ID == item.Id && c.layout_id == i.Id).Take(10).ToList();
                    KeyValuePair<layout, List<cards>> cardKVP = new KeyValuePair<layout, List<Models.cards>>(i, tmpList);
                    cards.Add(cardKVP);
                    }
                }
               
                
            }
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = new ApplicationUser();
            if (userName != null)
            {
                user = userManager.FindByNameAsync(userName).Result;
            }
          
            List<favorite> favorites = new List<favorite>();
            if (user != null)
            { 
                favorites = _entities.favorite.Where(c => c.user_id == user.Id).ToList();
            }
            ViewBag.favorites = favorites;
            ViewBag.cards = cards;
            return View(applicationDAO.getAllItemTypes());
        }

        public ActionResult Coordinates()
        {
            return View();
        }

        //Возвращает макеты заданного вида(визитки/листовки)
        public ActionResult ItemLayouts(string id)
        {
            try {
                if (id != null)
                {
                    return View(applicationDAO.getItemLayouts(id));
                }
                else return View("Error in HomeController.ItemLayouts");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return View("Error in HomeController.ItemLayouts");
            }
        }


        //Расширенный поиск
        public ActionResult exSearch(string id)
        {           
            if (id != null)
            { 
            var Search = new Search();    
            Search.typeId = id;            
            var exSearch = new ExtandedSearchModel();         
            var availItems = new List<checkBoxListModel>();          
            var selectedItems = new List<checkBoxListModel>();
            var claims = applicationDAO.getAllCardClaims().Where(c => c.published == true);
            int count = 0;
            foreach (var item in claims)
            {
                if (item.ClaimType != "TYPE")
                {
                    availItems.Add(new checkBoxListModel(count, item.ClaimValue, item.ClaimType));
                }
                count++;
            }
            Search.availItems = availItems;
            Session["itemTypeSession"] = id;
            var cards = applicationDAO.getItemLayouts(id).Where(c => c.published == true);
            foreach (var i in cards)
            {
                Search.Cards.Add(i);
            }             
            return View(Search);
            }
            else
            {
                return RedirectToAction("ItemTypes");
            }
        }

        //Расширенный поиск
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult exSearch(PostedItems postedItems)
        {
            string id = (string)Session["itemTypeSession"];
            var model = new Search();
            var selectedItems = new List<checkBoxListModel>();
            var selectedCards = new List<cards>();
            var postedItemsIds = new string[0];
            var availItems = new List<checkBoxListModel>();

            if (postedItems == null) postedItems = new PostedItems();

            if (postedItems.IDs != null && postedItems.IDs.Any())
            {
                postedItemsIds = postedItems.IDs;
            }
            if (postedItemsIds.Any())
            {
                selectedCards = applicationDAO.cardsForExtandedSearchList(postedItems.IDs, id).Where(cd => cd.published == true).Distinct().ToList();
                var claimsQuery = applicationDAO.selectedItemsForExtandedSearch(postedItems.IDs).ToList();
                int c = 0;
                foreach (var i in claimsQuery)
                {
                    selectedItems.Add(new checkBoxListModel(c, i.ClaimValue, i.ClaimType));
                }
            }
            else
            {
                selectedCards = applicationDAO.getItemLayouts(id).Where(c => c.published == true).ToList();
            }
          
           

            var claims = applicationDAO.getAllCardClaims().Where(c => c.published == true);
            int count = 0;
            foreach (var item in claims)
            {
                if (item.ClaimType != "TYPE")
                {
                    availItems.Add(new checkBoxListModel(count, item.ClaimValue, item.ClaimType));
                }
                count++;
            }
            model.availItems = availItems;
            model.selectedItems = selectedItems;
            model.postedItems = postedItems;
            model.Cards = selectedCards;
            Session["itemTypeSession"] = id;
            return View(model);
        }

        public ActionResult getItemTypeCards(int? id)
        {           
            if (id != null)
            {
                cards card = _entities.cards.Find(id);
                List<cards> cardsQuery = _entities.cards.Where(c => c.layout.name == card.layout.name && c.ITEM_TYPE.NAME == card.ITEM_TYPE.NAME && c.published == true).ToList();
                var Search = new Search();
                String itemTypeId = card.ITEM_TYPE.NAME;
                Search.typeId = itemTypeId;
                var exSearch = new ExtandedSearchModel();
                var availItems = new List<checkBoxListModel>();
                var selectedItems = new List<checkBoxListModel>();
                var claims = applicationDAO.getAllCardClaims().Where(c => c.published == true);
                int count = 0;
                foreach (var item in claims)
                {
                    if (item.ClaimType != "TYPE")
                    {
                        availItems.Add(new checkBoxListModel(count, item.ClaimValue, item.ClaimType));
                    }
                    count++;
                }
                Search.availItems = availItems;
                Session["itemTypeSession"] = itemTypeId;
                //var cards = applicationDAO.getItemLayouts(itemTypeId).Where(c => c.published == true);
                //foreach (var i in cards)
                //{
                //    Search.Cards.Add(i);
                //}
                Search.Cards.AddRange(cardsQuery);
                return View("exSearch", Search);
            }
            else
            {
                return RedirectToAction("ItemTypes");
            }
           
        }

     


        protected bool Validate(application application)
        {
            if (application.count_id <= 0)
            {
                ModelState.AddModelError("Count", "Count field shouldn't be empty");
            }
          
            return ModelState.IsValid;
        }
     
        
        public string getApplyServiceId(int materialId, int count)
        {

            materials material = _entities.materials.Find(materialId);
            COUNT c = _entities.COUNT.Find(count);
            decimal? resultPrice = material.price * c.COUNT_VALUE * c.PRICE;
            decimal res = Decimal.Round((decimal)resultPrice, 2);
            return "" + res;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
           // applicationDAO.getAllLayout();
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            applicationDAO.getAllLayout();
            return RedirectToAction("SetCanvas");
        }

        [AllowAnonymous]
        public ActionResult SetCanvas(int? id)
        {
            try {               
                if (id != null)
                {
                    cards card = _entities.cards.Find(id);
                    string itemType = card.ITEM_TYPE.NAME;
                    if (card != null)
                    {
                        card.IMAGES.Image = readFile(card.IMAGES.Image);
                        card.IMAGES.Image_2 = readFile(card.IMAGES.Image_2);
                        Dictionary<int, string> layoutO = new Dictionary<int, string>();
                        var layouts = _entities.layout.Where(c => c.ITEM_TYPE.NAME == itemType);
                        foreach (var item in layouts)
                        {
                            string param = item.width + " " + item.height;
                            layoutO.Add(item.Id, param);
                        }
                        ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
                        ViewBag.backgrounds = new List<elements>(_entities.elements.Where(c => c.type_name_id == 1).ToList());
                        ViewBag.info = new List<elements>(_entities.elements.Where(c => c.type_name_id == 2).ToList());
                        return View(card);
                    }
                    else
                    {
                        throw new NullReferenceException("cards is null");
                    }
                }
                else
                {
                    return RedirectToAction("ItemTypes");
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine(e.TargetSite.ToString());
                Console.WriteLine(e.StackTrace);
                return View("Error");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine(e.TargetSite.ToString());
                Console.WriteLine(e.StackTrace);
                return RedirectToAction("ItemTypes");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                Console.WriteLine(e.TargetSite.ToString());
                Console.WriteLine(e.StackTrace);
                return View("Error");
            }
        }

        [Authorize(Roles = "user, admin, manager")]
        [HttpPost]
        [ValidateInput(false)]
        [MultipleButton(Name = "action", Argument = "Submit")]
        public ActionResult SetCanvas(cards model)
        {
            if (ModelState.IsValid)
            { 
            cards cd = new cards();
            cd.IMAGES = new IMAGES();
            cd.IMAGES.Image = writeFile(model.IMAGES.Image);
                cd.IMAGES.Image_2 = writeFile(model.IMAGES.Image_2);
                cd.ITEM_TYPE_ID = model.ITEM_TYPE_ID;
                cd.layout_id = model.layout_id;
                cd.preview_2 = model.preview_2;
                cd.preview = model.preview;
                cd.text = model.text;
                cd.published = false;
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            string userId = user.Id;
            //model.layout = _entities.layout.Find(model.layout_id);
            cd.USER_ID = userId;
            _entities.cards.Add(cd);
            _entities.SaveChanges();
            return RedirectToAction("Apply", new { id = cd.Id });
            }
            return View("error");
        }

        private string readFile(string path)
        {
            if (path != null)
            { 
            string text = null;
            using (StreamReader sr = new StreamReader(path))
            {
                text = sr.ReadToEnd();
            }
                return text;
            }
            return "path is null";
        }

        private string writeFile(string text)
        {
            if (text != null)
            {
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

        [Authorize(Roles = "user, admin, manager")]
        public ActionResult Apply(int? id)
        {
            application app = new application();
            if (id != null) {
                cards card = _entities.cards.Find(id);;
                List<COUNT> counttList = _entities.COUNT.Where(c => c.ITEM_TYPE_ID == card.ITEM_TYPE_ID).ToList();
                Dictionary<int, String> countDic = new Dictionary<int, string>();
                foreach (var i in counttList)
                {
                    string param = i.COUNT_VALUE + " " + i.PRICE + "руб/шт";
                    countDic.Add(i.Id, param);
                }
                ViewBag.count_id = new SelectList(_entities.COUNT, "Id", "COUNT_VALUE");
                //ViewBag.material_id = new SelectList(_entities.materials, "Id", "description");
                ViewBag.towns = new SelectList(_entities.Towns, "Id", "Name");
                app.vc_id = card.Id;
                app.cards = card;
                return View(app);
            }
            else
            {
                return View("error Apply get");
            }
           
        }


        [Authorize(Roles = "user, admin, manager")]
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult SaveTo(cards model)
        {
            if (ModelState.IsValid)
            {
                cards card = model;
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                string userId = user.Id;
                //model.layout = _entities.layout.Find(model.layout_id);
                card.USER_ID = userId;
                //card.published = false;
                //applicationDAO.AddNewCard(card);
                _entities.cards.Add(card);
                _entities.SaveChanges();
            }
            return RedirectToAction("UserCards");
        }

        public ActionResult SendImage()
        {
            string image = (String)Session["imageSession"];
            return Json(image, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles="user, admin, manager")]
        public ActionResult Apply(application model)
        {
            if (ModelState.IsValid)
            {
                model.COUNT = _entities.COUNT.Find(model.count_id);
                //model.cards = _entities.cards.Find(model.vc_id);
                model.summ = model.COUNT.COUNT_VALUE * model.COUNT.PRICE;
                model.insertionDate = DateTime.Today;
                model.finishDate = DateTime.Today.AddDays(7);
                TempData["model"] = model;
                //_entities.application.Add(model);
                return RedirectToAction("YandexPayment", "Home");         
            }
            else 
            {
                return View("Error in apply post");
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        [Authorize(Roles = "user, admin, manager")]

        public ActionResult YandexPayment()
        {
            try { 
            application model = (application)TempData["model"];           
            return View(model);
            }
            catch (Exception e) 
            {
                Console.Out.WriteLine(e.TargetSite.ToString());
                Console.Out.WriteLine(e.Message.ToString());
                Console.Out.WriteLine(e.StackTrace);
                return View("Error");
            }
        }

        [HttpPost]
        [ActionName("YandexPayment")]
        public ActionResult YandexPayment(application model)
        {  
            try { 
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                string userId = user.Id;
                model.user_id = userId;
                model.status = "Обработка заявки";
                model.COUNT = _entities.COUNT.Find(model.count_id);
                model.cards = _entities.cards.Find(model.vc_id);
                _entities.application.Add(model);
                if (_entities.SaveChanges() < 0)
                {
                    Exception e = new Exception("Application is not inserted");
                        throw e;
                }           
                    // Секретный ключ интернет-магазина
                    string merchantKey = "4f5e327835605c44417c605e76745d707151375c5a396e59366775";
                    // Добавление полей формы в словарь, сортированный по именам ключей.
                    SortedDictionary<string, string> formField
                        = new SortedDictionary<string, string>();
                    formField.Add("WMI_MERCHANT_ID", "135519988118");
                    formField.Add("WMI_PAYMENT_AMOUNT", model.summ.ToString());
                    formField.Add("WMI_CURRENCY_ID", "643");
                    formField.Add("WMI_PAYMENT_NO", model.Id.ToString());
                    formField.Add("WMI_DESCRIPTION", "BASE64:" + Convert.ToBase64String(Encoding.UTF8.GetBytes("Оплата заказа " + model.Id + "Заказ: " + model.cards.ITEM_TYPE.NAME + "Колличество:" + _entities.COUNT.Find(model.count_id) + " с сайта profdesign.com")));
                    formField.Add("WMI_SUCCESS_URL", "https://profdesign.pro/Home/Paid");
                    formField.Add("WMI_FAIL_URL", "https://profdesign.pro/Home/Fail");
                    formField.Add("WMI_PTENABLED", "MasterCard");
                    //formField.Add("MyShopParam1", "Value1"); // Дополнительные параметры
                    //formField.Add("MyShopParam2", "Value2"); // магазина тоже участвуют
                    //formField.Add("MyShopParam3", "Value3"); // при формировании подписи!
                    // Формирование сообщения, путем объединения значений формы, 
                    // отсортированных по именам ключей в порядке возрастания и
                    // добавление к нему "секретного ключа" интернет-магазина
                    StringBuilder signatureData = new StringBuilder();
                    foreach (string key in formField.Keys)
                    {
                        if (formField.ContainsKey(key))
                        {
                            signatureData.Append(formField[key]);
                        }
                    }
                    // Формирование значения параметра WMI_SIGNATURE, путем 
                    // вычисления отпечатка, сформированного выше сообщения, 
                    // по алгоритму MD5 и представление его в Base64
                    string message = signatureData.ToString() + merchantKey;
                    Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(message);
                    Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
                    string signature = Convert.ToBase64String(hash);
                    // Добавление параметра WMI_SIGNATURE в словарь параметров формы
                    formField.Add("WMI_SIGNATURE", signature);
                    // Формирование платежной формы
                    StringBuilder output = new StringBuilder();
                    output.AppendLine("<form method=\"POST\" action=\"https://wl.walletone.com/checkout/checkout/Index\" accept-charset=\"UTF-8\">");
                    foreach (var key in formField.Keys)
                    {
                        //string str = String.Format("{0}={1}", key, formField[key]);
                        //if (!key.Equals(last))
                        //{
                        //    str += "&";
                        //}
                        //output.AppendLine(str);
                        if (key == "wmi_payment_amount")
                        {
                            string str = String.Format("<input name={0} value={1} readonly=\"readonly\"/><br>", key, formField[key]);
                            output.AppendLine(str);
                        }
                        else if (key == "wmi_description")
                        {
                            string str = String.Format("<input name={0} value={1} type=\"hidden\" id =\"description\" readonly=\"readonly\"/><br>", key.ToString(), formField[key]);
                            output.AppendLine(str);
                        }
                        else
                        {
                            string str = String.Format("<input name={0} value={1} type=\"hidden\" readonly=\"readonly\"/><br>", key.ToString(), formField[key]);

                            output.AppendLine(str);
                        }
                    }
                    //var content = new HttpContent(output.ToString());
                    //var content = new FormUrlEncodedContent(formField);
                    //ASCIIEncoding encoding = new ASCIIEncoding();
                    //byte[] data = encoding.GetBytes(output.ToString());
                    var context = HttpContext; //Current
                    //var client = new HttpClient();
                    //client.BaseAddress = new Uri("https://localhost:44303");
                    output.AppendLine("<input id =\"subm\" type=\"submit\"/></form>");
                    //HttpWebRequest rqst = (HttpWebRequest)WebRequest.Create("https://wl.walletone.com/checkout/checkout/Index");
                    //rqst.Method = "POST";
                    //rqst.ContentType = "application/x-www-form-urlencoded";
                    //rqst.ContentLength = output.Length;
                    ViewBag.htmloutput = output.ToString();
                    //Stream newStream = rqst.GetRequestStream();
                    //// Send the data.
                    //newStream.Write(data, 0, data.Length);
                    //newStream.Close();
                    context.Response.ContentType = "text/html; charset=UTF-8";
                    context.Response.Write(output.ToString());
                    //string rrr =  content.ReadAsStringAsync().ToString();
                    //var res = client.PostAsync("https://wl.walletone.com/checkout/checkout/Index", content).Result;
                    //var stringContent = res.Content.ReadAsStringAsync();
                    //var nvc = new NameValueCollection();
                    //foreach (var i in formField.Keys)
                    //{
                    //    var name = String.Format("{0}", i.ToString(), formField[i]);
                    //    var value = String.Format("{1}", i.ToString(), formField[i]);
                    //    nvc.Add(name, value);
                    //}
                    //using (WebClient wclient = new WebClient())
                    //{

                    //    byte[] response =
                    //    wclient.UploadValues("https://wl.walletone.com/checkout/checkout/Index", nvc);

                    //    string result = System.Text.Encoding.UTF8.GetString(response);
                    //}
                    //string result = GetWebResponse("https://wl.walletone.com/checkout/checkout/Index", nvc);
                    return View("PaymentSubmit");

                }
                else
                {
                    return View("Error");
                }   
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.TargetSite.ToString());
                Console.Out.WriteLine(ex.Message.ToString());
                Console.Out.WriteLine(ex.Source.ToString());
                Console.Out.WriteLine(ex.StackTrace);
                return View("Error");
            }
            }


        static string GetWebResponse(string url, NameValueCollection parameters)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";

            var sb = new StringBuilder();
            foreach (var key in parameters.AllKeys)
                sb.Append(key + "=" + parameters[key] + "&");
            sb.Length = sb.Length - 1;

            byte[] requestBytes = Encoding.UTF8.GetBytes(sb.ToString());
            httpWebRequest.ContentLength = requestBytes.Length;

            using (var requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
            }

            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null);
            using (var responseStream = responseTask.Result.GetResponseStream())
            {
                var reader = new StreamReader(responseStream);
                return reader.ReadToEnd();
            }
        }

        [HttpPost]
        public string Fail()
        {
            string mkey = "4f5e327835605c44417c605e76745d707151375c5a396e59366775";
            foreach (string key in Request.Form.Keys)
            {
                if (!Request.Form.AllKeys.Contains("WMI_SIGNATURE"))
                {
                    sendResponse("Retry", "There is no WMI_SIGNATURE");
                    return "There is no WMI_SIGNATURE";
                }
                if (!Request.Form.AllKeys.Contains("WMI_PAYMENT_NO"))
                {
                    sendResponse("Retry", "There is no WMI_PAYMENT_NO");
                    return "There is no WMI_PAYMENT_NO";
                }
                if (!Request.Form.AllKeys.Contains("WMI_ORDER_STATE"))
                {
                    sendResponse("Retry", "There is no WMI_ORDER_STATE");
                    return "There is no WMI_ORDER_STATE";
                }
            }
            return "Что то пошло не так" + "Внутренний номер заявки" + Request.Form["WMI_PAYMENT_NO"] + "Статус заявки"
                + Request.Form["WMI_ORDER_STATE"];
        }

        public class DuplicateKeyComparer<T>
                 :
              IComparer<T> where T : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(T x, T y)
            {
                
                int result = (x.ToString()).CompareTo(y.ToString());
                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }

            #endregion
        }


        [Authorize(Roles = "user, admin, manager")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
          

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Authorize(Roles = "user, admin, manager")]
        //public ActionResult UserApplication()
        //{
        //    string userName = User.Identity.Name;
        //    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
        //    var userManager = new UserManager<ApplicationUser>(store);
        //    ApplicationUser user = userManager.FindByNameAsync(userName).Result;
        //    string userId = user.Id;
        //    ViewData["UserApplication"] = applicationDAO.getUserApplications(userId);
        //    IEnumerable<application> appQuery = applicationDAO.getUserApplications(userId);
        //    List<application> appList = new List<application>();
        //    foreach (var i in appList)
        //    {
        //        string status = i.status;
        //        if (!status.Equals("Обработка заявки"))
        //        {
        //            ViewBag.deleteStyle = "display:none";
        //        }
        //    }
        //   appList.AddRange(appQuery);
        //    Session["UserApplication"] = appList;
        //    return View(ViewData["UserApplication"]);
        //}

        //[Authorize(Roles = "user, admin, manager")]
        //public ActionResult UserCards()
        //{
        //    return View("UserCards");
        //}


        //[Authorize(Roles ="user, admin, manager")]
        //public ActionResult CardsList()
        //{
        //    string userName = User.Identity.Name;
        //    var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
        //    var userManager = new UserManager<ApplicationUser>(store);
        //    ApplicationUser user = userManager.FindByNameAsync(userName).Result;
        //    string userId = user.Id;
        //    var cards = applicationDAO.getUserCards(userId);         
        //    return PartialView("Cards", cards.ToList());
        //}


        [Authorize(Roles = "user, admin, manager")]
        public ActionResult ApplicationDetails(int id)
        {
            IEnumerable<application> appList = applicationDAO.getApplication(id);
            List<application> appQuery = new List<application>();
            appQuery.AddRange(appList);
            //application app = new application();
            int vc_id = 0;
            foreach (var i in appQuery)
            {
                vc_id = (int)i.vc_id;
            }
            IEnumerable<cards> cardQuery = applicationDAO.getCardById(vc_id);
            List<cards> cardList = new List<cards>();
            cardList.AddRange(cardQuery);
            string path = null;
            foreach (var i in cardList)
            {
                path = i.IMAGES.Image;
            }           
            //path = path.Replace(@"\", "/");
            ViewBag.ImageData = path; 
            return PartialView("ApplicationDetails");
        }



        [AllowAnonymous]
        public void SendEmailConfirmation(ApplicationUser user)
        {
            // наш email с заголовком письма
            MailAddress from = new MailAddress("platon.5070@gmail.com", "Web Registration");
            // кому отправляем
            MailAddress to = new MailAddress(user.UserName);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Email confirmation";
            // текст письма - включаем в него ссылку
            m.Body = string.Format("Конструктор визиток ххххх: ваша заявка готова!");
            m.IsBodyHtml = true;
            // адрес smtp-сервера, с которого мы и будем отправлять письмо
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);

            smtp.UseDefaultCredentials = false;
            // логин и пароль
            smtp.Credentials = new System.Net.NetworkCredential("platon.5070@gmail.com", "stop290390");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(m);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Delete(int id)
        {
            applicationDAO.Delete(id);
            return RedirectToAction("ManagerApplicationList");
        }

        public PartialViewResult Load()
        {
            return PartialView("ApplicationDetails");
        }




        [Authorize(Roles = "admin, manager")]
        public ActionResult Manager()
        {
            return View("Manager");
        }



        [Authorize(Roles = "admin")]
        public ActionResult AddAdmin(string Id)
        {                    
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
           // ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            ApplicationUser user = userManager.FindById(Id);
           // int userId = user.UsersForeignKey;
            var role1 = new IdentityRole { Name = "admin" };
            userManager.AddToRole(user.Id, role1.Name);
            
            return RedirectToAction("UserList");
        }


        [Authorize(Roles = "admin")]
        public ActionResult UserList()
        {
            Entities1 UsersContext = new Entities1();
            var users = UsersContext.AspNetUsers.ToList();
            ViewData["Users"] = UsersContext.AspNetUsers.ToList();
            return View(UsersContext.AspNetUsers.ToList());
        }


        [Authorize(Roles = "admin")]
        public ActionResult UserInfo(int id)
        {
            IEnumerable<application> tmpApp = applicationDAO.getApplication(id);
            string user_id = "";
            foreach (var i in tmpApp)
            {
                user_id = i.user_id;
            }
            Entities1 UsersContext = new Entities1();
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            var users = UsersContext.AspNetUsers.ToList();
            AspNetUsers user = new AspNetUsers();
            foreach (var i in users)
            {
                if (i.Id == user_id)
                {
                    user = i;
                }
            }
            List<AspNetUsers> userQuery = new List<AspNetUsers>();
            userQuery.Add(user);
            //IEnumerable<AspNetUsers> tmpUser = userQuery;
            ViewData["manageUser"] = userQuery;
            return View(userQuery);
        }


        [Authorize(Roles = "admin, superadmin")]
        public ActionResult AdminUserDetails(string Id)
        {
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            Entities1 UsersContext = new Entities1();
            var userManager = new UserManager<ApplicationUser>(store);
            // ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            ApplicationUser user = userManager.FindById(Id);
            var roles = UsersContext.AspNetRoles.ToList();
            List<string> rolesQuery = new List<string>();
            foreach (var i in roles)
            {
                rolesQuery.Add(i.Name);
            }
            ViewBag.roles = new SelectList(rolesQuery);
            Dictionary<ApplicationUser, List<AspNetRoles>> addUserToRoleList = new Dictionary<ApplicationUser, List<AspNetRoles>>();
            addUserToRoleList.Add(user, roles);
            return View(addUserToRoleList);
        }



        [Authorize(Roles = "admin")]
        public ActionResult AddToRole(string Id, string role)
        {
            string userName = User.Identity.Name;
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            // ApplicationUser user = userManager.FindByNameAsync(userName).Result;
            ApplicationUser user = userManager.FindById(Id);
            var currRole = userManager.GetRoles(Id).SingleOrDefault();
            if (currRole != null)
            {
                userManager.RemoveFromRoles(Id, currRole);
            }
            userManager.AddToRole(user.Id, role);
            return RedirectToAction("UserList");
        }


        //Edit user's layout
        [Authorize(Roles = "user, admin, manager")]
        public ActionResult EditLayout(int id)
        {
            cards card = new cards();
            IEnumerable<cards> cardQuery = applicationDAO.getCardById(id);
            foreach (var i in cardQuery)
            {
                card.IMAGES.Image = i.IMAGES.Image;
            }
            ViewBag.EditLayoutId = id;
            ViewBag.EditLayout = card.IMAGES.Image;
            return View("EditLayout");
        }

      


        //[Authorize(Roles = "user, admin, manager")]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult EditLayout(String imageData, int EditCardId)
        //{
        //    applicationDAO.EditCardLayout(EditCardId, imageData);
        //    return RedirectToAction("UserApplication");
        //}

        [Authorize(Roles = "user, admin, manager")]
        public ActionResult EditLayoutNew(String imageData)
        {
            application application = new application();
            cards card = new cards();
            if (ModelState.IsValid)
            {
                string userName = User.Identity.Name;
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(userName).Result;
                string userId = user.Id;
                card.USER_ID = userId;
                card.layout_id = 1;
                card.IMAGES.Image = imageData;
                applicationDAO.AddNewCard(card);
                return RedirectToAction("UserApplication");
            }
            else
            {
                return View("EditLayout");
            }
        }


        //Upload new card layout
        public ActionResult UploadCardLayout()
        {
            return View("UploadCardLayout");
        }

        [HttpPost]
        public string Paid()
        {
            string mkey = "4f5e327835605c44417c605e76745d707151375c5a396e59366775";
            foreach (string key in Request.Form.Keys)
            {
                if (!Request.Form.AllKeys.Contains("WMI_SIGNATURE"))
                {
                    sendResponse("Retry", "There is no WMI_SIGNATURE");
                    return "There is no WMI_SIGNATURE";
                }
                if (!Request.Form.AllKeys.Contains("WMI_PAYMENT_NO"))
                {
                    sendResponse("Retry", "There is no WMI_PAYMENT_NO");
                    return "There is no WMI_PAYMENT_NO";
                }
                if (!Request.Form.AllKeys.Contains("WMI_ORDER_STATE"))
                {
                    sendResponse("Retry", "There is no WMI_ORDER_STATE");
                    return "There is no WMI_ORDER_STATE";
                }
            }

            SortedDictionary<string, string> formField
                   = new SortedDictionary<string, string>();

            foreach (string key in Request.Form.Keys)
            {
                if (key != "WMI_SIGNATURE")
                { 
                formField.Add(key, HttpUtility.UrlDecode(Request.Form[key]));
                }
            }

            StringBuilder signatureData = new StringBuilder();

            foreach (string key in formField.Keys)
            {
                if (formField.ContainsKey(key))
                {
                    signatureData.Append(formField[key]);
                }
            }

            // Формирование значения параметра WMI_SIGNATURE, путем 
            // вычисления отпечатка, сформированного выше сообщения, 
            // по алгоритму MD5 и представление его в Base64

            string message = signatureData.ToString() + mkey;
            Byte[] bytes = Encoding.GetEncoding(1251).GetBytes(message);
            Byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string signature = Convert.ToBase64String(hash);

            
            if (signature == Request.Form["WMI_SIGNATURE"])
            {
                if (Request.Form["WMI_ORDER_STATE"].ToUpper() == "ACCEPTED")
                {
                    sendResponse("OK", "Order " + Request.Form["WMI_PAYMENT_NO"] + "is paid!");
                    //Add payment amount to database
                    application order = _entities.application.FirstOrDefault(o => o.Id == Int32.Parse(Request.Form["WMI_PAYMENT_NO"]));
                    order.operation_id = Request.Form["WMI_ORDER_ID"];
                    order.insertionDate = DateTime.Now;
                    order.amount = Decimal.Parse(Request.Form["WMI_PAYMENT_AMOUNT"]);
                    order.withdrawamount = Decimal.Parse(Request.Form["WMI_COMISSION_AMOUNT"]);
                    order.sender = Request.Form["WMI_TO_USER_ID"];
                    _entities.Entry(order).State = EntityState.Modified;
                    _entities.SaveChanges();
                    return "<p>заказ оплачен</p>" + "Оплачено: " + Request.Form["WMI_PAYMENT_AMOUNT"] + "Комиссия: " + Request.Form["WMI_COMISSION_AMOUNT"];               
                }
                else
                {
                    //Something went wrong. Wrong WMI_ORDER_STATE
                    sendResponse("Retry", "Wrong WMI_ORDER_STATE" + Request.Form["WMI_ORDER_STATE"]);
                    return "Wrong WMI_ORDER_STATE" + Request.Form["WMI_ORDER_STATE"];
                }
            }
            else
            {
                //Signature doesnt match
                sendResponse("Retry", "Wrong WMI_SIGNATURE" + Request.Form["WMI_SIGNATURE"]);
                return "Wrong WMI_SIGNATURE" + Request.Form["WMI_SIGNATURE"];
            }
            
        }

        public void sendResponse(string status, string description)
        {
            Response.Write("WMI_RESULT=" + status.ToUpper() + "&");
            Response.Write("WMI_DESCRIPTION" + HttpUtility.UrlEncode(description));    
        }
      

        public string GetHash(string val)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(val));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


        public ActionResult AllNewCard()
        {
            string id = (string)Session["itemTypeSession"];
            cards card = new cards();
            if (id != null)
            {
               
                int item_type_id = _entities.ITEM_TYPE.Where(c => c.NAME == id).Select(c => c.Id).SingleOrDefault();
                card.ITEM_TYPE_ID = item_type_id;
               
                Dictionary<int, string> layoutO = new Dictionary<int, string>();
                var layouts = _entities.layout.Where(c => c.ITEM_TYPE.NAME == id);
                foreach (var item in layouts)
                {
                    string param = item.width + " " + item.height;
                    layoutO.Add(item.Id, param);
                }
                ViewBag.layout_id = new SelectList(layoutO, "Key", "Value");
                ViewBag.backgrounds = new List<elements>(_entities.elements.Where(c => c.type_name_id == 1).ToList());
                ViewBag.info = new List<elements>(_entities.elements.Where(c => c.type_name_id == 2).ToList());
                //ViewBag.item_type_id = item_type_id;
            }
            return View(card);
        }

        //[HttpPost]
        //public ActionResult AllNewCard(cards card)
        //{
        //    Session["card"] = card;
        //    return RedirectToAction("Apply");
        //}
    }


    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
}
