namespace TatBlog.WebApi.Models
{
    public class SubscriberFilterModel: PagingModel
    {
        public string Email { get; set; }
        public bool ForceLock { get; set; }
        public bool UnsubscribeVoluntary { get; set; }
    }
}
