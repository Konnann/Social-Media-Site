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


            //List<Post> taggedPosts = new List<Post>();
            //var posts = db.Posts;
            ////var p = db.Posts.Where(x => x.Id == 2).Include(x => x.Tags);

            //Tag tag = db.Tags.Single(t => t.TagId == 4);
            //var tagPosts = db.Posts.ToList();
            //tagPosts = tagPosts.Where(p => p.Tags.Contains(tag)).ToList();

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
                /*
                    HttpWebResponse response = null;
                    var request = ( HttpWebRequest ) WebRequest.Create (post.Body);
                    request.Method = "HEAD";


                    try
                    {
                        response = ( HttpWebResponse ) request.GetResponse ( );
                        postsExist.Add (true);
                    }
                    catch ( WebException ex )
                    {


                    postsExist.Add (false);
                }
                    finally
                {
                    // Don't forget to close your response.
                    if ( response != null )
                    {
                        response.Close ( );
                    }

                }
                */

            }

            ViewBag.IsYoutubeVideo = isYoutubeVideo;
            ViewBag.PostExists = postsExist;
            ViewBag.TaggedPosts = posts.Distinct();
            return View(posts.ToList());
        }


        public ActionResult Index()
        {



            var posts = db.Posts.Include(p => p.Author).OrderByDescending(p => p.Date);
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