using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class TagEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên thẻ")]
        public string Name { get; set; }

        [DisplayName("Slug")]
        [Remote("VerifyTagSlug", "Tags", "Admin", HttpMethod = "POST")]
        public string UrlSlug { get; set; }

        [DisplayName("Nội dung")]
        public string Description { get; set; }
    }
}
