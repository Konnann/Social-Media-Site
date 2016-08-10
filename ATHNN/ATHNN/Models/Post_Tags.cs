using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ATHNN.Models
{
    public class Post_Tags
    {
        [ForeignKey("PostId")]
        public int PostId { get; set; }

        [ForeignKey("TagId")]
        public int TagId { get; set; }

        public virtual ICollection<Post> PostID { get; set; }

        public virtual ICollection<Tag> TagID { get; set; }
    }
}