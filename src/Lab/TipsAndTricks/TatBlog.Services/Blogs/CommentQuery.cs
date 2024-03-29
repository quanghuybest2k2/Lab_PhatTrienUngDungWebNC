﻿using System;
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
        public string UserName { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string PostTitle { get; set; }
        public bool Censored { get; set; }
    }
}
