using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
    public class SubscriberFilterModel
    {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Email tác giả")]
        public string Email { get; set; }

        [DisplayName("Buộc khóa")]
        public bool ForceLock { get; set; }

        [DisplayName("Tự nguyện hủy")]
        public bool UnsubscribeVoluntary { get; set; }
    }
}
