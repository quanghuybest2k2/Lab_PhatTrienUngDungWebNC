using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Blogs
{
    public class AuthorQuery
    {
        public string Keyword { get; set; }
        public string FullName { get; set; }
        public string UrlSlug { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
    }
}
