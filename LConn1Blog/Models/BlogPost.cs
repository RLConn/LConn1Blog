using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LConn1Blog.Models
{
    public class BlogPost
    {

        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Title { get; set; }
        //This property will be used to display a short snippet to the user
        public string Abstract { get; set; }
        //This slug will be produced behind the scenes using the Title
        public string Slug { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaURL { get; set; }
        public bool Published { get; set; }

        //Virtual Nav
        public virtual ICollection<Comment> Comments { get; set; }

        public BlogPost()
        {
            this.Comments = new HashSet<Comment>();
        }
    }
}