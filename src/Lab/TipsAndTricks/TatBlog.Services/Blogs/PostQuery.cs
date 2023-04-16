using TatBlog.Core.Contracts;

namespace TatBlog.Services.Blogs
{
    public class PostQuery
    {
        //mã tác giả, mã chuyên mục, tên ký hiệu chuyên mục, năm/tháng đăng bài, từ khóa, … 
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string AuthorSlug { get; set; }
        public string TagSlug { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string PostSlug { get; set; }
        public bool PublishedOnly { get; set; } = true;
        public string Keyword { get; set; }
        public string TitleSlug { get; set; }
        public bool UnPublished { get; set; }
        public IList<string> SelectedTag { get; set; }

        public void GetTagListAsync()
        {
            SelectedTag = (TagSlug ?? "").Split(new[] { ",", ";", ".", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
