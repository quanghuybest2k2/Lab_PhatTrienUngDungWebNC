using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
    }
}
