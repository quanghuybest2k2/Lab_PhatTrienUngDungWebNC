using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class CommentEditModel
    {
        public int Id { get; set; }

        [DisplayName("Phê duyệt")]
        public bool IsApproved { get; set; }

        [DisplayName("Bài viết")]
        public int postId { get; set; }

        public IEnumerable<SelectListItem>? PostList { get; set; }
    }
}
