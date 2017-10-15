using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TechnicalTest.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;



namespace TechnicalTest.Controllers
{
    
    public class HomeController : Controller
    {
        List<User> contactList;
        public object UserManager { get; private set; }
        public object SignInManager { get; private set; }
        public object SignInStatus { get; private set; }
        public object Files { get; private set; }
        public object Paths { get; private set; }
        public object StandardCharsets { get; private set; }


        public HomeController()
        {
            Initialize();
        }
        private void Initialize()
        {
            using (var webClient = new WebClient())
            {
                // WebProxy wp = new WebProxy("your proxy url",false);
                webClient.Encoding = Encoding.UTF8;
                // webClient.Proxy = wp;
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Headers[HttpRequestHeader.Allow] = "Get";
                Rootobject rootObject = new Rootobject();
                var json = webClient.DownloadString("http://jsonplaceholder.typicode.com/users");
                rootObject.Users2 = JsonConvert.DeserializeObject<List<User>>(json.ToString());
                rootObject.Users = JsonConvert.DeserializeObject<User[]>(json.ToString());
                contactList= rootObject.Users2.ToList();
            }
        }

        //[HttpPost]
        [Authorize]
        public ActionResult Index(string search,string SortOrder)
        {
            List<User> searchResult = new List<User>();
            ViewBag.name = string.IsNullOrEmpty(SortOrder) ? "name_desc" : "";
            ViewBag.username = string.IsNullOrEmpty(SortOrder) ? "username_desc" : "username";
            ViewBag.email = string.IsNullOrEmpty(SortOrder) ? "email_desc" : "email";
            //contactList= Initialize();
            switch (SortOrder)
            {
                case "name_desc":
                    contactList = contactList.OrderByDescending(s => s.name).ToList();
                    break;
                case "username_desc":
                    contactList = contactList.OrderByDescending(s => s.username).ToList();
                    break;
                case "username":
                    contactList = contactList.OrderBy(s => s.username).ToList();
                    break;
                case "email_desc":
                    contactList = contactList.OrderByDescending(s => s.email).ToList();
                    break;
                case "email":
                    contactList = contactList.OrderBy(s => s.email).ToList();
                    break;
                default:
                    contactList = contactList.OrderBy(s => s.name).ToList();
                    break;
            }
            if (string.IsNullOrEmpty(search))
                searchResult = contactList.ToList();
            else
                searchResult = contactList.Where(x => x.name.ToLower().StartsWith(search.ToLower())).ToList();
            if(searchResult.Count==0)
                ViewBag.Text ="No Match Item";
            ViewData["user"] = User.Identity.Name;
            return View(searchResult);
        }

        public ActionResult Details(int? id)
        {
            User user = contactList.FirstOrDefault(x => x.id == id.Value);
            return View(user);
        }

        public ActionResult Report()
        {
            List<string> startingChar = new List<string>();
            List<string> reapetedChar = new List<string>();
            List<int> number=new List<int>();
            int i = 0;
            foreach (User user in contactList)
                {
                    startingChar.Add( user.name.Substring(0, 1).ToString());
                 }
            reapetedChar = startingChar.Distinct().ToList();
            foreach (string item in reapetedChar)
            {                
                number.Add(contactList.Count(s => s.name.StartsWith(reapetedChar[i])));
                i++;
            }
            ViewBag.charList = reapetedChar;
            ViewBag.number = number;
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}