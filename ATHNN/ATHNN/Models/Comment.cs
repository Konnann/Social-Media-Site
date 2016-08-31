using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATHNN.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }

        [Required]
        public int PostId { get; set; }

        //[Required]
        public int AuthorId { get; set; }

        //[Required]
        public string AuthorName { get; set; }


        [Required]
        public DateTime Date { get; set; }

        public ApplicationUser Author { get; set; }
        public Post Post { get; set; }

    }
}