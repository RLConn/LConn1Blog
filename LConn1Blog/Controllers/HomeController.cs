using LConn1Blog.Models;
using LConnBlog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace LConn1Blog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(EmailModel email)
        {
            var from = $"{email.FromEmail}<{WebConfigurationManager.AppSettings["emailfrom"]}>";
            var myEmail = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
            {
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(myEmail);

            return RedirectToAction("Home", "Index");
        }                           
        
    }
}