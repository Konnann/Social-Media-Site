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
using Microsoft.Owin.Security.OAuth.Messages;

namespace ATHNN.Controllers
{
    [ValidateInput (false)]
    public class PostsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext ( );

        // Post: LikePost
        public ActionResult LikePost(int postId)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var currentPost = db.Posts.FirstOrDefault(p => p.Id == postId);

            //add likes
            //TODO: Check if post is already liked and if it is - unlike
            UserPostLikedPair userPostPair = new UserPostLikedPair();
            userPostPair.user = currentUser;
            userPostPair.post = currentPost;

           // if (!db.UserPostPairs.Contains(userPostPair))
            //{
                db.Posts.Find(postId).Likes += 1;
            //}
            db.UserPostPairs.Add(userPostPair);
            db.UserPostPairs.Include(p => p.post);
            db.SaveChanges();

            return RedirectToAction("../Home/Index");
        }

        public ActionResult LikedPosts()
        {
            ApplicationUser currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Post> likedPosts = new List<Post>();
            var pairs = db.UserPostPairs.Include(p=>p.user).Include(p => p.post).Distinct();

            foreach (UserPostLikedPair up in pairs)
            {
                if (up.post != null)
                {
                    if (up.user.Id == currentUser.Id && !likedPosts.Contains(up.post))
                    {
                        Post post = db.Posts.Find(up.post.Id);
                        likedPosts.Add(post);
                    }
                }
            }


            UserPostViewModel model = new UserPostViewModel
            {
                CurrentUser = currentUser,
                AllPosts = likedPosts
            };

            return View(model);
        }
        static public List<Post> UserPosts(ApplicationUser currentUser, List<Post> allPosts)
        {
            //exclude posts without authors to evade null reference error
            List<Post> postsWithAuthors = allPosts.Where(p => p.Author != null).ToList();

            //check remaining posts
            List<Post> userPosts = allPosts.Where(p => p.Author.UserName == currentUser.UserName).OrderByDescending(p => p.Date).ToList();
            return (userPosts);
        }

        // GET: Posts
        [Authorize]
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
            Post post = db.Posts.Include(p => p.Author).Include(p => p.Comments).SingleOrDefault(x => x.Id == id);
            var comments = db.Comments.Include(p=>p.Author).Where(cm => cm.PostId == post.Id).OrderByDescending(cm => cm.Date).ToList();
            post.Comments = comments;
            
            if(post == null)
            {
                return HttpNotFound();
            }
            bool postExists = HomeController.IsValidURI(post.Body);
            ViewBag.PostExists = postExists;
            return View(post);

        }

        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind (Include = "Id,Title,Body,TagString,Tags")] Post post)
        {
            if (ModelState.IsValid)
            {

                List<string> taglist = post.TagString.Split(' ').ToList();
                foreach ( var tagl in taglist )
                {
                    post.Tags.Add(new Tag() { Name = tagl });
                }
                post.Author = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
                post.Date = DateTime.Now;
                db.Posts.Add (post);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind (Include = "Id,Title,Body,TagString,Tags")] Post post)
        {

            Post thisPost = this.db.Posts.Include(p => p.Tags).FirstOrDefault(p => p.Id == post.Id);
                
            if (ModelState.IsValid)
            {

                thisPost.Tags.Clear();

                db.SaveChanges();

                List<string> taglist = post.TagString.Split(' ').ToList();

                foreach (var tagl in taglist)
                {
                    thisPost.Tags.Add(new Tag() { Name = tagl });
                }
                thisPost.Title = post.Title;
                thisPost.Body = post.Body;
                thisPost.Date = DateTime.Now;
                thisPost.TagString = post.TagString;
                thisPost.Author = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);
                db.Entry(thisPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction ("Index","Home");
            }
            return View (post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public ActionResult Delete( int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find (id);
            if(post == null)
            {
                return HttpNotFound();
            }
            return View (post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName ("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
