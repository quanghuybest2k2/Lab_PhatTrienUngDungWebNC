using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs
{
    public class PostQuery: IPostQuery
    {
        //mã tác giả, mã chuyên mục, tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, … 
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool PublishedOnly { get; set; } = true;
        public string Keyword { get; set; }
        public string TitleSlug { get; set; }
        public bool NotPublished { get; set; }
    }
}
