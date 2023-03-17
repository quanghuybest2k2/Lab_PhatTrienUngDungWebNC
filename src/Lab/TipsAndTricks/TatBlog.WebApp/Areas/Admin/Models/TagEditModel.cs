using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        public string? Name { get; set; }

        [DisplayName("Slug")]
        public string? UrlSlug { get; set; }

        [DisplayName("Giới thiệu")]
        public string? Description { get; set; }
    }
}
