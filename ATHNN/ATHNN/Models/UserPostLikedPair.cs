using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATHNN.Models
{
    public class UserPostLikedPair
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public ApplicationUser user { get; set; }
        [Key]
        [Required]
        public Post post { get; set; }

    }
}