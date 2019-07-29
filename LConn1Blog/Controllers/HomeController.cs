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
using PagedList;
using PagedList.Mvc;


namespace LConn1Blog.Controllers
{
    [RequireHttps]
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var myPost = IndexSearch(searchStr);

            var pageSize = 5;
            var pageNumber = page ?? 1;

            return View(myPost.ToPagedList(pageNumber, pageSize));
        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            var result = db.BlogPosts.Where(b=>b.Published);
            if (searchStr != null)
            {
                result = result.Where(p => p.Title.Contains(searchStr) ||
                    p.Abstract.Contains(searchStr) ||
                    p.Body.Contains(searchStr) ||
                    p.Comments.Any(c => c.CommentBody.Contains(searchStr) ||
                                    c.Author.FirstName.Contains(searchStr) ||
                                    c.Author.LastName.Contains(searchStr) ||
                                    c.Author.DisplayName.Contains(searchStr) ||
                                    c.Author.Email.Contains(searchStr)));
            }
            return result.OrderByDescending(p => p.Created);
        }


        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
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