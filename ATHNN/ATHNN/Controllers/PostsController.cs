using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using ATHNN.Models;

namespace ATHNN.Controllers
{
    [ValidateInput (false)]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext ( );

        // GET: Posts
        public ActionResult Index()
        {
            var postsWithAuthors = db.Posts.Include(p => p.Author).ToList();
            return View (postsWithAuthors);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p=>p.Author).SingleOrDefault(x=>x.Id==id);
            
            if (post == null)
            {
                return HttpNotFound();
            }
            bool postExists = HomeController.IsValidURI(post.Body);
            ViewBag.PostExists = postExists;
            return View(post);

        }

        // GET: Posts/Create
        public ActionResult Create ( )
        {
            return View ( );
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create ( [Bind (Include = "Id,Title,Body,TagString,Tags")] Post post )
        {
            if ( ModelState.IsValid )
            {

                List<string> taglist = post.TagString.Split (' ').ToList ( );
                foreach ( var tagl in taglist )
                {
                    post.Tags.Add (new Tag ( ) { Name = tagl });
                }
                post.Author = db.Users.FirstOrDefault (user => user.UserName == User.Identity.Name);
                post.Date = DateTime.Now;
                db.Posts.Add (post);
                db.SaveChanges ( );
                return RedirectToAction ("Index");
            }

            return View (post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit ( int? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find (id);
            if ( post == null )
            {
                return HttpNotFound ( );
            }
            return View (post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit ( [Bind (Include = "Id,Title,Body,Tags")] Post post )
        {
            if ( ModelState.IsValid )
            {
                post.Date = DateTime.Now;
                db.Entry (post).State = EntityState.Modified;
                db.SaveChanges ( );
                return RedirectToAction ("Index");
            }
            return View (post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete ( int? id )
        {
            if ( id == null )
            {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find (id);
            if ( post == null )
            {
                return HttpNotFound ( );
            }
            return View (post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed ( int id )
        {
            Post post = db.Posts.Find (id);
            db.Posts.Remove (post);
            db.SaveChanges ( );
            return RedirectToAction ("Index");
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                db.Dispose ( );
            }
            base.Dispose (disposing);
        }
    }
}
