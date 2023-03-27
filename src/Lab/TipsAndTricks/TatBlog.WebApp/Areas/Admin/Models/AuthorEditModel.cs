using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class AuthorEditModel
    {
        public int Id { get; set; }

        [DisplayName("Tên tác giả")]
        public string FullName { get; set; }

        [DisplayName("Slug")]
        [Remote("VerifyAuthorSlug", "Authors", "Admin", HttpMethod = "POST", AdditionalFields = "Id")]
        public string UrlSlug { get; set; }

        [DisplayName("Chọn hình ảnh")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Hình hiện tại")]
        public string ImageUrl { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Ghi chú")]
        public string Notes { get; set; }
    }
}
