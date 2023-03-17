using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CommentFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Bài viết")]
        public int? PostId { get; set; }

        [DisplayName("Năm")]
        public int? PostedYear { get; set; }

        [DisplayName("Tháng")]
        public int? PostedMonth { get; set; }

        [DisplayName("Chưa phê duyệt")]
        public bool IsNotApproved { get; set; }

        public IEnumerable<SelectListItem> PostList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public CommentFilterModel()
        {
            MonthList = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem()
                {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                })
                .ToList();
        }
    }
}
