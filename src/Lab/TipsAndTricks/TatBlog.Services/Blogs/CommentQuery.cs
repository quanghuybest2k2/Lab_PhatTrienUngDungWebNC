using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs
{
    public class CommentQuery : ICommentQuery
    {
        public string Keyword { get; set; }
        public int? PostedMonth { get; set; }
        public int? PostedYear { get; set; }
        public bool IsNotApproved { get; set; }
        public int? PostId { get; set; }
    }
}
