using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Blogs
{
    public class PostQuery
    {
        //mã tác giả, mã chuyên mục, tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, … 
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string UrlSlug { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
