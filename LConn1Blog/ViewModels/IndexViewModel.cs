using LConn1Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LConn1Blog.ViewModels
{
    public class IndexVM
    {
        public BlogPost FeaturedPost { get; set; }
        public ICollection<BlogPost> RecentPosts { get; set; }
        public ICollection<Comment> RecentComments { get; set; }
        public ICollection<ApplicationUser> NewUsers { get; set; }
        public IndexVM()
        {
            RecentPosts = new List<BlogPost>();
            RecentComments = new List<Comment>();
            NewUsers = new List<ApplicationUser>();
        }
    }
}
