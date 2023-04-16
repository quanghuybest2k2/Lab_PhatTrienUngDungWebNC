using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApi.Models
{
    public class PostFilterModel: PagingModel
    {
        [DisplayName("Từ khoá")]
        public string Keyword { get; set; }
        [DisplayName("Đã xuất bản")]
        public bool PublishedOnly { get; set; }
        [DisplayName("Không xuất bản")]
        public bool UnPublished { get; set; }
        [DisplayName("Năm")]
        public int? Year { get; set; }
        [DisplayName("Tháng")]
        public int? Month { get; set; }
        [DisplayName("Post slug")]
        public string PostSlug { get; set; }
        [DisplayName("Author slug")]
        public string AuthorSlug { get; set; }
        [DisplayName("Category slug")]
        public string CategorySlug { get; set; }
        [DisplayName("Tag slug")]
        public string TagSlug { get; set; }
        [DisplayName("Tác giả")]
        public int? AuthorId { get; set; }
        [DisplayName("Chủ đề")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> AuthorList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }

        public PostFilterModel()
        {
            MonthList = Enumerable.Range(1, 12).Select(m => new SelectListItem() { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) }).ToList();
        }
    }
}
