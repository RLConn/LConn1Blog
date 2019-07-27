using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LConn1Blog.Helpers;
using LConn1Blog.Models;
using LConn1Blog.ViewModels;

namespace LConn1Blog.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        [AllowAnonymous]
        public ActionResult Index()
        {
            //db.BlogPosts.ToList()
            //db.BlogPosts.Where(b => b.Published).ToList()
            var publishedPosts = db.BlogPosts.Where(b => b.Published).OrderByDescending(b => b.Created).ToList();
            return View(publishedPosts);
        }

        public ActionResult AdvancedIndex()
        {
            var recentSixPosts = db.BlogPosts.Where(b => b.Published).OrderByDescending(b => b.Created).Take(6);

            //var myData = new IndexVM();
            //myData.FeaturedPost = recentSixPosts.FirstOrDefault();
            //myData.RecentPosts = recentSixPosts.Skip(1).ToList();
            //myData.RecentComments = db.Comments.OrderByDescending(c => c.Created).Take(5).ToList();
            //myData.NewUsers = db.Users.OrderByDescending(u => u.Created).Take(5).ToList();

            var myData = new IndexVM
            {
                FeaturedPost = recentSixPosts.FirstOrDefault(),
                RecentPosts = recentSixPosts.Skip(1).ToList(),
                RecentComments = db.Comments.OrderByDescending(c => c.Created).Take(5).ToList(),
                NewUsers = db.Users.OrderByDescending(u => u.Created).Take(5).ToList()
            };

            return View(myData);
        }

        // GET: BlogPosts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BlogPost blogPost = db.BlogPosts.Find(id);
        //    if (blogPost == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(blogPost);
        //}
        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Abstract,Slug,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var slug = StringUtilities.URLFriendly(blogPost.Title);

                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.BlogPosts.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }
                blogPost.Slug = slug;
                blogPost.Created = DateTimeOffset.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                
                
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Abstract,Slug,Created,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if(ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }

                db.Entry(blogPost).State = EntityState.Modified;
                blogPost.Updated = DateTimeOffset.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
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
