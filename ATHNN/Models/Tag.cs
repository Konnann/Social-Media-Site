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

        public List<Post> Posts { get; set; }
    }
}