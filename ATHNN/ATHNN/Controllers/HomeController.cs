using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATHNN.Models;

namespace ATHNN.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext ( );

        public static bool IsValidURI ( string uri )
        {
            if ( !Uri.IsWellFormedUriString (uri, UriKind.Absolute) )
                return false;
            Uri tmp;
            if ( !Uri.TryCreate (uri, UriKind.Absolute, out tmp) )
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }

        public static bool IsYouTubeVideo(string url)
        {
            string[] URLSplit = url.Split('.').ToArray();
            if (URLSplit.Contains("youtube"))
            {
                return true;
            }
            return false;
        }

        public ActionResult Gaming ()
        {
            // Listing tagged posts 

            string[] tagsForListing = new string[] { "game", "gaming" };

            IQueryable<Post> postByTag = db.Posts.Include(p=>p.Author).OrderByDescending(p=>p.Date);
            List<Post> posts = new List<Post> ( );
            foreach ( var tagText in tagsForListing )
            {
                posts.AddRange (postByTag.Where (p => p.Tags.Select (t => t.Name).Contains (tagText)).ToList ( ));
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool> ();
            List<bool> isYoutubeVideo = new List<bool>();
            foreach(var post in posts)
            {
                if(IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }

                if ( IsYouTubeVideo (post.Body))
                {
                    isYoutubeVideo.Add(true);
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }

        public ActionResult SoftUni()
        {
            // Listing tagged posts 

            string[] tagsForListing = new string[] { "softuni", "software","university" };

            IQueryable<Post> postByTag = db.Posts.Include(p => p.Author).OrderByDescending(p => p.Date);
            List<Post> posts = new List<Post>();
            foreach (var tagText in tagsForListing)
            {
                posts.AddRange(postByTag.Where(p => p.Tags.Select(t => t.Name).Contains(tagText)).ToList());
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool>();
            List<bool> isYoutubeVideo = new List<bool>();
            foreach (var post in posts)
            {
                if (IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }

                if (IsYouTubeVideo(post.Body))
                {
                    isYoutubeVideo.Add(true);
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }
        public ActionResult GIFS()
        {
            // Listing tagged posts 

            string[] tagsForListing = new string[] { "GIF", "gifche", "gif4e" };

            IQueryable<Post> postByTag = db.Posts.Include(p => p.Author).OrderByDescending(p => p.Date);
            List<Post> posts = new List<Post>();
            foreach (var tagText in tagsForListing)
            {
                posts.AddRange(postByTag.Where(p => p.Tags.Select(t => t.Name).Contains(tagText)).ToList());
            }

            //Check if image is existing
            List<bool> postsExist = new List<bool>();
            List<bool> isYoutubeVideo = new List<bool>();
            foreach (var post in posts)
            {
                if (IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }

                if (IsYouTubeVideo(post.Body))
                {
                    isYoutubeVideo.Add(true);
                }
                else
                {
                    isYoutubeVideo.Add(false);
                }
            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }

        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author).OrderByDescending(p => p.Date);
            var topFiveCommentedPosts =
                db.Posts.Include(p => p.Comments).
                OrderByDescending(p=>p.Comments.Count).
                Take(5);
            ViewBag.TopFiveCommented = topFiveCommentedPosts;
            List<bool> isYoutubeVideo = new List<bool> ( );
            List<bool> postsExist = new List<bool> ( );
            foreach (var post in posts)
            {
                if (IsValidURI(post.Body))
                {
                    postsExist.Add(true);
                }
                else
                {
                    postsExist.Add(false);
                }
                if ( IsYouTubeVideo (post.Body) )
                {
                    isYoutubeVideo.Add (true);
                }
                else
                {
                    isYoutubeVideo.Add (false);
                }
            }
            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.SideBarPosts = posts;
            ViewBag.PostExists = postsExist;
            return View(posts.ToList());
        }

    }
}