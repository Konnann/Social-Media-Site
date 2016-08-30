using ATHNN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace ATHNN.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult MyPosts()
        {
            //Select all posts from logged in user
            var userPosts = db.Posts.Where(p => p.Author.UserName == User.Identity.Name).Include(p => p.Author).OrderByDescending(p => p.Date).ToList();

            List<bool> postsExist = new List<bool>();
            foreach (var post in userPosts)
            {
                if (HomeController.IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }
            }
            ViewBag.PostExists = postsExist;
            TempData["PostExists"] = postsExist;

            return View(userPosts);
        }
    }
}