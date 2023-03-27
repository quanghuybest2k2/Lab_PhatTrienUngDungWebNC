using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CommentFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Tên độc giả")]
        public string UserName { get; set; }

        [DisplayName("Năm")]
        public int? Year { get; set; }

        [DisplayName("Tháng")]
        public int? Month { get; set; }

        [DisplayName("Ngày")]
        public int? Day { get; set; }

        [DisplayName("Bài viết")]
        public string PostTitle { get; set; }

        [DisplayName("Đã Phê duyệt")]
        public bool Censored { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }

        public CommentFilterModel()
        {
            MonthList = Enumerable.Range(1, 12)
                                  .Select(m => new SelectListItem()
                                  {
                                      Value = m.ToString(),
                                      Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                                  }).ToList();
        }
    }
}
