using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs
{
    public class CategoryQuery : ICategoryQuery
    {
        public string Keyword { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
