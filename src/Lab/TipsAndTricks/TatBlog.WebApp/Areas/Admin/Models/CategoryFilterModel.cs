using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CategoryFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
        [DisplayName("Hiển thị trên Menu")]
        public bool ShowOnMenu { get; set; }
    }
}
