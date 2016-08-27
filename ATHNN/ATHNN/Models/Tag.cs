using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATHNN.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Posts=new List<Post>();
        }
        [Key]
        public int TagId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }

        //public override string ToString()
        //{
        //    var t = new Tag() { TagId = 1, Name = "name", Posts = new List<Post>() { } };
        //    var t1 = new Tag() { TagId = 1, Name = "name", Posts = new List<Post>() { } };
        //    var t2 = new Tag() { TagId = 1, Name = "name", Posts = new List<Post>() { } };

        //    List<Tag> tags = new List<Tag>() {t,t1,t2};

        //    string output = "";

        //    foreach (var tag in tags)
        //    {
        //        output += String.Format("Tag Nomer {0}, s ime {1}, posove: ");
        //        foreach (var post in tag.Posts)
        //        {
        //            output += post.Title;
        //        }
        //        output += "\n";
        //    }
        //    return output;
        //}
    }
}