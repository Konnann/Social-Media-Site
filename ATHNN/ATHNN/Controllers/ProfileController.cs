using ATHNN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace ATHNN.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        // POST: Profile
        [HttpPost]
        [Authorize]
        public ActionResult UploadProfilePicture(HttpPostedFileBase file)
        {

            if (file != null)
            {
                //get name of file
                string name = System.IO.Path.GetFileName(file.FileName);

                byte[] image;
                //copy to byte array so that we can save it in db
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    image = ms.GetBuffer();
                }

                db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).ProfilePicture = image;
                db.SaveChanges();
            }

            return RedirectToAction("ProfileTemplate");
        }

        // GET: Profile/ProfileTemplate
        public ActionResult ProfileTemplate()
        {
            ApplicationUser currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            List<Post> userPosts = PostsController.UserPosts(currentUser, db.Posts.Include(p => p.Author).ToList());
            UserPostViewModel model = new UserPostViewModel
                                          {
                                              CurrentUser = currentUser,
                                              AllPosts = userPosts
                                          };

            return View(model);
        }
    }
}