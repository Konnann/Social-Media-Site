using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATHNN.Models
{
    public class Post
    {
        public Post()
        {
            this.Tags = new List<Tag>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public string TagString { get; set; }

        public ApplicationUser Author { get; set; }
        public List<Comment> Comments { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}