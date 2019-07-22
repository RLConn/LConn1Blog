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
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.BlogPosts.Where(b => b.Published).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
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

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return RedirectToAction("Index","Home");



            //return View(model);
        }                           
        
    }
}